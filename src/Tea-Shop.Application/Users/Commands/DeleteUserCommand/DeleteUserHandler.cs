using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.DeleteUserCommand;

public class DeleteUserHandler
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

    public async Task<Result<Guid, Error>> Handle(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _usersRepository.DeleteUser(new UserId(userId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogInformation("Failed to delete user {productId}", userId);

            return deleteResult.Error;
        }

        _logger.LogInformation("User with id {UserId} deleted.", userId);

        return userId;
    }
}