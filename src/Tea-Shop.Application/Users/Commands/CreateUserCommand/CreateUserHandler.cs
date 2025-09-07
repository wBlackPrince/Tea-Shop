using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.FilesStorage;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.CreateUserCommand;

public class CreateUserHandler: ICommandHandler<
    CreateUserResponseDto,
    CreateUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IValidator<CreateUserRequestDto> _validator;
    private readonly IFileProvider _fileProvider;

    private const string _avatarBucket = "media";

    public CreateUserHandler(
        IUsersRepository usersRepository,
        ILogger<CreateUserHandler> logger,
        IFileProvider fileProvider,
        IValidator<CreateUserRequestDto> validator)
    {
        _usersRepository = usersRepository;
        _logger = logger;
        _fileProvider = fileProvider;
        _validator = validator;
    }

    public async Task<Result<CreateUserResponseDto, Error>> Handle(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Error.Validation(
                "create.user",
                "validation failed",
                validationResult.Errors.First().PropertyName);
        }

        bool isEmailUnique = await _usersRepository.IsEmailUnique(command.Request.Email, cancellationToken);

        if (!isEmailUnique)
        {
            return Error.Failure("create.user", "email is already taken");
        }

        var userId = new UserId(Guid.NewGuid());


        Guid? avatarMediaId = null;
        string? avatarKey = null;

        if (command.Request.FileDto is not null)
        {
            // расширение файла
            var ext = Path.GetExtension(command.Request.FileDto.FileName);

            // путь + имя внутри бакета
            var key = $"users/{userId.Value}/avatars/{Guid.NewGuid():N}{ext}";

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
                return Error.Failure("create.user", $"avatar upload failed: {upload.Error.Message}");
            }

            var media = upload.Value;

            avatarKey = media.Key;
            avatarMediaId = media.Id;
        }

        User user = new User(
            userId,
            command.Request.Password,
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Email,
            command.Request.PhoneNumber,
            (Role)Enum.Parse(typeof(Role), command.Request.Role),
            avatarMediaId,
            command.Request.MiddleName);

        await _usersRepository.CreateUser(user, cancellationToken);

        await _usersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with id {UserId} created a new account.", user.Id.Value);

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