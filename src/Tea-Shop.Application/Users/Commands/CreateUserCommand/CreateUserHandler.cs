using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.FilesStorage;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Buskets;
using Tea_Shop.Domain.Users;
using Tea_Shop.Infrastructure.Postgres.Repositories;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.CreateUserCommand;

public class CreateUserHandler: ICommandHandler<
    CreateUserResponseDto,
    CreateUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IBusketsRepository _busketsRepository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IValidator<CreateUserRequestDto> _validator;
    private readonly IFileProvider _fileProvider;
    private readonly ITransactionManager _transactionManager;

    private const string _avatarBucket = "media";

    public CreateUserHandler(
        IUsersRepository usersRepository,
        IBusketsRepository busketsRepository,
        ILogger<CreateUserHandler> logger,
        IFileProvider fileProvider,
        IValidator<CreateUserRequestDto> validator,
        ITransactionManager transactionManager)
    {
        _usersRepository = usersRepository;
        _busketsRepository = busketsRepository;
        _logger = logger;
        _fileProvider = fileProvider;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<CreateUserResponseDto, Error>> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError(validationResult.Errors.ToString());

            return Error.Validation(
                "create.user",
                "validation failed",
                validationResult.Errors.First().PropertyName);
        }



        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        bool isEmailUnique = await _usersRepository.IsEmailUnique(command.Request.Email, cancellationToken);

        if (!isEmailUnique)
        {
            _logger.LogError($"email {command.Request.Email} already exists");
            transactionScope.Rollback();
            return Error.Failure("create.user", "email is already taken");
        }

        var userId = new UserId(Guid.NewGuid());


        Guid? avatarId = Guid.NewGuid();

        User user = new User(
            userId,
            command.Request.Password,
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Email,
            command.Request.PhoneNumber,
            (Role)Enum.Parse(typeof(Role), command.Request.Role),
            avatarId,
            command.Request.MiddleName);

        await _usersRepository.CreateUser(user, cancellationToken);


        // создание корзины для пользователя
        BusketId busketId = new BusketId(Guid.NewGuid());
        Busket busket = new Busket(busketId, user.Id);

        await _busketsRepository.Create(busket, cancellationToken);



        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while creating user");
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

            var upload = await _fileProvider.UploadAsync(
                stream: s,
                key: key,
                bucket: _avatarBucket,
                fileName: command.Request.FileDto.FileName,
                createBucketIfNotExists: true,
                cancellationToken: cancellationToken);

            if (upload.IsFailure)
            {
                _logger.LogError($"avatar upload failed: {upload.Error.Message}");
                return Error.Failure("create.user", $"avatar upload failed: {upload.Error.Message}");
            }
        }

        _logger.LogDebug("User with id {UserId} created a new account.", user.Id.Value);

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