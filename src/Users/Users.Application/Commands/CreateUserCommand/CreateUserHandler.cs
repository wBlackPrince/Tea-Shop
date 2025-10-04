using System.Data;
using CSharpFunctionalExtensions;
using FluentEmail.Core;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Contracts;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.FilesStorage;
using Shared.ValueObjects;
using Users.Contracts;
using Users.Contracts.Dtos;
using Users.Domain;

namespace Users.Application.Commands.CreateUserCommand;

public class CreateUserHandler(
    IUsersRepository usersRepository,
    IBasketsRepository basketsRepository,
    ITokensRepository tokensRepository,
    ILogger<CreateUserHandler> logger,
    IFileProvider fileProvider,
    IValidator<CreateUserRequestDto> validator,
    ITransactionManager transactionManager,
    IFluentEmail fluentEmail,
    EmailVerificationLinkFactory verificationLinkFactory,
    IPasswordHasher passwordHasher): ICommandHandler<CreateUserResponseDto, CreateUserCommand>
{
    const string _avatarBucket = "media";

    public async Task<Result<CreateUserResponseDto, Error>> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            logger.LogError(validationResult.Errors.ToString());

            return Error.Validation(
                "create.user",
                "validation failed",
                validationResult.Errors.First().PropertyName);
        }



        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        bool isEmailUnique = await usersRepository.IsEmailUnique(command.Request.Email, cancellationToken);

        if (!isEmailUnique)
        {
            logger.LogError($"email {command.Request.Email} already exists");
            transactionScope.Rollback();
            return Error.Failure("create.user", "email is already taken");
        }

        var userId = new UserId(Guid.NewGuid());


        Guid? avatarId = Guid.NewGuid();

        BasketId basketId = new BasketId(Guid.NewGuid());

        User user = new User(
            userId,
            passwordHasher.Hash(command.Request.Password),
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Email,
            command.Request.PhoneNumber,
            (Role)Enum.Parse(typeof(Role), command.Request.Role),
            basketId,
            avatarId,
            command.Request.MiddleName);

        DateTime utcNow = DateTime.UtcNow;
        var verificationToken = new EmailVerificationToken()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CreatedOnUtc = utcNow,
            ExpiresOnUtc = utcNow.AddDays(1),
            User = user,
        };

        await tokensRepository.CreateVerificationTokenToken(verificationToken, cancellationToken);

        await usersRepository.CreateUser(user, cancellationToken);

        Basket basket = new Basket(basketId, userId);
        
        await basketsRepository.Create(basket, cancellationToken);

        // email verification
        string verificationLink = verificationLinkFactory.Create(verificationToken);

        await fluentEmail
            .To(user.Email)
            .Subject("Email verification for TeaShop")
            .Body($"To verify your email address, click here <a href='{verificationLink}'>click here</a>", isHtml: true)
            .SendAsync();



        var saveResult = await transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
        {
            logger.LogError(saveResult.Error.ToString());
            transactionScope.Rollback();
            return saveResult.Error;
        }


        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        if (command.Request.FileDto is not null)
        {
            // расширение файла
            var ext = Path.GetExtension(command.Request.FileDto.FileName);

            // путь + имя внутри бакета
            var key = $"users/{userId.Value}/avatars/{avatarId:N}{ext}";

            await using var s = command.Request.FileDto.Stream;

            var upload = await fileProvider.UploadAsync(
                stream: s,
                key: key,
                bucket: _avatarBucket,
                fileName: command.Request.FileDto.FileName,
                createBucketIfNotExists: true,
                cancellationToken: cancellationToken);

            if (upload.IsFailure)
            {
                logger.LogError($"avatar upload failed: {upload.Error.Message}");
                return Error.Failure("create.user", $"avatar upload failed: {upload.Error.Message}");
            }
        }


        logger.LogDebug("User with id {UserId} created a new account.", user.Id.Value);

        var response = new CreateUserResponseDto()
        {
            Id = user.Id.Value,
            Password = command.Request.Password,
            FirstName = command.Request.FirstName,
            LastName = command.Request.LastName,
            Email = command.Request.Email,
            PhoneNumber = command.Request.PhoneNumber,
            Role = user.Role.ToString(),
            AvatarId = user.AvatarId,
            MiddleName = user.MiddleName,
        };

        return response;
    }
}