using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;
using Shared.ValueObjects;
using Users.Domain;

namespace Users.Application.Commands.UpdateUserCommand;

public class UpdateUserHandler(
    IUsersRepository usersRepository,
    ILogger<UpdateUserHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid userId,
        JsonPatchDocument<User> userUpdates,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handleName}", nameof(UpdateUserHandler));


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while updating user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var user = await usersRepository.GetUserById(
            new UserId(userId),
            cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User not found while updating");
            transactionScope.Rollback();
            return Error.NotFound("update.user", "user not found");
        }

        try
        {
            userUpdates.ApplyTo(user);
        }
        catch (Exception e)
        {
            logger.LogError("Validation error while updating product");
            transactionScope.Rollback();
            return Error.Validation("update product", e.Message);
        }



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogDebug("Update user with id {UserId}", userId);

        return user.Id.Value;
    }
}