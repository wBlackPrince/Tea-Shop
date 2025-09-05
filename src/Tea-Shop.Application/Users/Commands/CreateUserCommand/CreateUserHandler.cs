using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.CreateUserCommand;

public class CreateUserHandler: ICommandHandler<CreateUserResponseDto, CreateUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<CreateUserHandler> _logger;
    private readonly IValidator<CreateUserRequestDto> _validator;

    public CreateUserHandler(
        IUsersRepository usersRepository,
        ILogger<CreateUserHandler> logger,
        IValidator<CreateUserRequestDto> validator)
    {
        _usersRepository = usersRepository;
        _logger = logger;
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

        User user = new User(
            new UserId(Guid.NewGuid()),
            command.Request.Password,
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Email,
            command.Request.PhoneNumber,
            (Role)Enum.Parse(typeof(Role), command.Request.Role),
            command.Request.AvatarId,
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