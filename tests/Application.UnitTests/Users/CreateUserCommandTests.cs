using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using Tea_Shop.Application.FilesStorage;
using Tea_Shop.Application.Users;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Application.UnitTests.Users;

public class CreateUserCommandTests
{
    private static readonly CreateUserCommand Command = new CreateUserCommand(
        new CreateUserRequestDto(
            "qwerty",
            "Edaurd",
            "Nikitin",
            "myemail@example.com",
            "73947484994",
            "USER",
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "Anatolyevich",
            null));

    private readonly CreateUserHandler _handler;

    private readonly IUsersRepository _usersRepositoryMock;

    private readonly ILogger<CreateUserHandler> _loggerMock;

    private readonly IValidator<CreateUserRequestDto> _validatorMock;

    private readonly IFileProvider _filesProviderMock;

    public CreateUserCommandTests()
    {
        _usersRepositoryMock = Substitute.For<IUsersRepository>();
        _loggerMock = Substitute.For<ILogger<CreateUserHandler>>();
        _validatorMock = Substitute.For<IValidator<CreateUserRequestDto>>();
        _filesProviderMock = Substitute.For<IFileProvider>();

        _handler = new CreateUserHandler(
            _usersRepositoryMock,
            _loggerMock,
            _filesProviderMock,
            _validatorMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsNotUnique()
    {
        // arrange
        _usersRepositoryMock.IsEmailUnique(
            Arg.Is<string>(e => e == Command.Request.Email),
            Arg.Any<CancellationToken>()).Returns(false);

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));


        // result
        var result = await _handler.Handle(Command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Failure("create.user", "email is already taken"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenPasswordIsEmpty()
    {
        // arrange
        var command = Command with
        {
            Request = Command.Request with { Password = "" }
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Password",
                    "Password is required")
            }));


        // result
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Validation(
                "create.user",
                "validation failed",
                "Password"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenFirstNameIsEmpty()
    {
        // arrange
        var command = Command with
        {
            Request = Command.Request with { FirstName = "" }
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "FirstName",
                    "First Name is required")
            }));


        // result
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Validation(
                "create.user",
                "validation failed",
                "FirstName"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenLastNameIsEmpty()
    {
        // arrange
        var command = Command with
        {
            Request = Command.Request with { LastName = "" }
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "LastName",
                    "Last Name is required")
            }));


        // result
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Validation(
                "create.user",
                "validation failed",
                "LastName"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenUserRoleIsInvalid()
    {
        // arrange
        var command = Command with
        {
            Request = Command.Request with { Role = "invalid" }
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Role",
                    "Role is not valid")
            }));


        // result
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Validation(
                "create.user",
                "validation failed",
                "Role"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenPhoneNumberIsEmpty()
    {
        // arrange
        var command = Command with
        {
            Request = Command.Request with { PhoneNumber = "" }
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "PhoneNumber",
                    "Phone Number is required")
            }));

        // result
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Validation(
                "create.user",
                "validation failed",
                "PhoneNumber"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsEmpty()
    {
        // arrange
        var command = Command with
        {
            Request = Command.Request with { Email = "" }
        };

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[]
            {
                new ValidationFailure(
                    "Email",
                    "Email is required")
            }));

        // result
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.ShouldBe(
            Error.Validation(
                "create.user",
                "validation failed",
                "Email"));
    }

    [Fact]
    public async Task Handle_Should_ReturnError_WhenEmailIsUnique()
    {
        // arrange
        _usersRepositoryMock.IsEmailUnique(
            Arg.Is<string>(e => e == Command.Request.Email),
            Arg.Any<CancellationToken>()).Returns(true);

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));


        // result
        var result = await _handler.Handle(Command, CancellationToken.None);

        // assert
        result.ShouldBe(new CreateUserResponseDto(){
            Id = result.Value.Id,
            Password = "qwerty",
            FirstName = "Edaurd",
            LastName = "Nikitin",
            Email = "myemail@example.com",
            PhoneNumber = "73947484994",
            Role = "USER",
            AvatarId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            MiddleName = "Anatolyevich",
        });
    }


    [Fact]
    public async Task Handle_Should_CallRepository_WhenValidationIsSuccessful()
    {
        // arrange
        _usersRepositoryMock.IsEmailUnique(
            Arg.Is<string>(e => e == Command.Request.Email),
            Arg.Any<CancellationToken>()).Returns(true);

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));

        // result
        var result = await _handler.Handle(Command, CancellationToken.None);

        // assert
        _usersRepositoryMock.Received(1).CreateUser(
            Arg.Is<User>(u => u.Id == new UserId(result.Value.Id)),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_Should_CallSaveChangeAsync_WhenValidationIsSuccessful()
    {
        // arrange
        _usersRepositoryMock.IsEmailUnique(
            Arg.Is<string>(e => e == Command.Request.Email),
            Arg.Any<CancellationToken>()).Returns(true);

        _validatorMock
            .ValidateAsync(Arg.Any<CreateUserRequestDto>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(new ValidationResult()));

        // result
        var result = await _handler.Handle(Command, CancellationToken.None);

        // assert
        _usersRepositoryMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}