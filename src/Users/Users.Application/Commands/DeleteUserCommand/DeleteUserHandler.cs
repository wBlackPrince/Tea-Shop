using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;
using Users.Contracts;

namespace Users.Application.Commands.DeleteUserCommand;

public class DeleteUserHandler(
    IUsersRepository usersRepository,
    ILogger<DeleteUserHandler> logger,
    ITransactionManager transactionManager):
    ICommandHandler<UserWithOnlyIdDto, DeleteUserCommand>
{
    public async Task<Result<UserWithOnlyIdDto, Error>> Handle(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(DeleteUserHandler));


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while deleting user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var deleteResult = await usersRepository.DeleteUser(
            new UserId(command.Request.UserId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            logger.LogInformation(
                "Failed to delete user {productId}",
                command.Request.UserId);

            transactionScope.Rollback();

            return deleteResult.Error;
        }


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while deleting user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogDebug("User with id {UserId} deleted.", command.Request.UserId);

        return command.Request;
    }
}