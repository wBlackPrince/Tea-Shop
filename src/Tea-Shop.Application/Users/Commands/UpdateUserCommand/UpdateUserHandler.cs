using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.UpdateUserCommand;

public class UpdateUserHandler
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<UpdateUserHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public UpdateUserHandler(
        IUsersRepository usersRepository,
        ILogger<UpdateUserHandler> logger,
        ITransactionManager transactionManager)
    {
        _usersRepository = usersRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }


    public async Task<Result<Guid, Error>> Handle(
        Guid userId,
        JsonPatchDocument<User> userUpdates,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handleName}", nameof(UpdateUserHandler));


        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while updating user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var user = await _usersRepository.GetUserById(
            new UserId(userId),
            cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User not found while updating");
            transactionScope.Rollback();
            return Error.NotFound("update.user", "user not found");
        }

        try
        {
            userUpdates.ApplyTo(user);
        }
        catch (Exception e)
        {
            _logger.LogError("Validation error while updating product");
            transactionScope.Rollback();
            return Error.Validation("update product", e.Message);
        }



        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while updating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        _logger.LogDebug("Update user with id {UserId}", userId);

        return user.Id.Value;
    }
}