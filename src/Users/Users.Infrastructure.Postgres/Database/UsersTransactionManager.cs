using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;

namespace Users.Infrastructure.Postgres.Database;

public class UsersTransactionManager : ITransactionManager
{
    private readonly UsersDbContext _dbContext;
    private readonly ILogger<UsersTransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public UsersTransactionManager(
        UsersDbContext dbContext,
        ILogger<UsersTransactionManager> logger,
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

            var logger = _loggerFactory.CreateLogger<UsersTransactionScope>();

            var transactionScope = new UsersTransactionScope(transaction.GetDbTransaction(), logger);

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