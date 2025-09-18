using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.DeleteUserCommand;

public class DeleteUserHandler:
    ICommandHandler<UserWithOnlyIdDto, DeleteUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<DeleteUserHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public DeleteUserHandler(
        IUsersRepository usersRepository,
        ILogger<DeleteUserHandler> logger,
        ITransactionManager transactionManager)
    {
        _usersRepository = usersRepository;
        _logger = logger;
        _transactionManager =  transactionManager;
    }

    public async Task<Result<UserWithOnlyIdDto, Error>> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteUserHandler));


        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while deleting user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var deleteResult = await _usersRepository.DeleteUser(
            new UserId(command.Request.UserId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogInformation(
                "Failed to delete user {productId}",
                command.Request.UserId);

            transactionScope.Rollback();

            return deleteResult.Error;
        }


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while deleting user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        _logger.LogDebug(
            "User with id {UserId} deleted.",
            command.Request.UserId);

        return command.Request;
    }
}