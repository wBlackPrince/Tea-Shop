using System.Transactions;
using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Error>> BeginTransactionAsync(CancellationToken cancellationToken);

    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}