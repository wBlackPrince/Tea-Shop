using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;
using IsolationLevel = System.Data.IsolationLevel;

namespace Users.Application.Commands.VerifyEmailCommand;

public class VerifyEmail(
    ITokensRepository tokensRepository,
    ILogger<VerifyEmail> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<bool, Error>> Handle(Guid tokenId, CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while verifying email");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;




        EmailVerificationToken? token = await tokensRepository.GetVerificationToken(tokenId, cancellationToken);

        if (token is null || token.ExpiresOnUtc < DateTime.UtcNow || token.User.EmailVerified)
        {
            return false;
        }

        token.User.EmailVerified = true;

        tokensRepository.RemoveVerificationToken(token);

        var saveResult = await transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
        {
            logger.LogError(saveResult.Error.ToString());
            transactionScope.Rollback();
            return saveResult.Error;
        }


        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while verifying email");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        return true;
    }
}