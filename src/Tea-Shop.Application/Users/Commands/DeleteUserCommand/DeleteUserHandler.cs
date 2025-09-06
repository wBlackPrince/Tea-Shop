using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.DeleteUserCommand;

public class DeleteUserHandler:
    ICommandHandler<UserWithOnlyIdDto, DeleteUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(
        IUsersRepository usersRepository,
        ILogger<DeleteUserHandler> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public async Task<Result<UserWithOnlyIdDto, Error>> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _usersRepository.DeleteUser(
            new UserId(command.Request.UserId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogInformation(
                "Failed to delete user {productId}",
                command.Request.UserId);

            return deleteResult.Error;
        }

        _logger.LogInformation(
            "User with id {UserId} deleted.",
            command.Request.UserId);

        return command.Request;
    }
}