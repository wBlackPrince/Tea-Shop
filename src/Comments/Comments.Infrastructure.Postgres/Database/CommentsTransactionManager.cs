using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;

namespace Commnets.Infrastructure.Postgres.Database;

public class CommentsTransactionManager : ITransactionManager
{
    private readonly CommentsDbContext _dbContext;
    private readonly ILogger<CommentsTransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public CommentsTransactionManager(
        CommentsDbContext dbContext,
        ILogger<CommentsTransactionManager> logger,
        ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task<Result<ITransactionScope, Error>> BeginTransactionAsync(
        IsolationLevel? isolationLevel,
        CancellationToken cancellationToken)
    {
        try
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(
                isolationLevel ?? IsolationLevel.ReadCommitted,
                cancellationToken);

            var logger = _loggerFactory.CreateLogger<CommentsTransactionScope>();

            var transactionScope = new CommentsTransactionScope(transaction.GetDbTransaction(), logger);

            return transactionScope;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "BeginTransactionAsync failed");
            return Error.Failure(
                "transaction",
                "Failed to begin transaction");
        }
    }

    public async Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,  "SaveChangesAsync failed");

            return Error.Failure("transaction", "Failed to save changes");
        }
    }
}