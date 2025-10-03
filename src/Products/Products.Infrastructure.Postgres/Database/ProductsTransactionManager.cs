using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;

namespace Products.Infrastructure.Postgres.Database;

public class ProductsTransactionManager : ITransactionManager
{
    private readonly ProductsDbContext _dbContext;
    private readonly ILogger<ProductsTransactionManager> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public ProductsTransactionManager(
        ProductsDbContext dbContext,
        ILogger<ProductsTransactionManager> logger,
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

            var logger = _loggerFactory.CreateLogger<ProductsTransactionScope>();

            var transactionScope = new ProductsTransactionScope(transaction.GetDbTransaction(), logger);

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