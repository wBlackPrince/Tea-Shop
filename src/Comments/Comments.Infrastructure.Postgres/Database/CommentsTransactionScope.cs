using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;

namespace Commnets.Infrastructure.Postgres.Database;

public class CommentsTransactionScope: ITransactionScope, IDisposable
{
    private readonly IDbTransaction _transaction;
    private readonly ILogger<CommentsTransactionScope> _logger;

    public CommentsTransactionScope(
        IDbTransaction transaction,
        ILogger<CommentsTransactionScope> logger)
    {
        _transaction = transaction;
        _logger = logger;
    }

    public UnitResult<Error> Commit()
    {
        try
        {
            _transaction.Commit();
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to commit transaction");

            return Error.Failure(
                "transaction",
                "Failed to commit transaction");
        }
    }

    public UnitResult<Error> Rollback()
    {
        try
        {
            _transaction.Rollback();

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to rollback transaction");

            return Error.Failure(
                "transaction",
                "Failed to rollback transaction");
        }
    }

    public void Dispose()
    {
        _transaction.Dispose();
    }
}