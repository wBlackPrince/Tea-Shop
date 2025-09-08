using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Database;

public interface ITransactionScope: IDisposable
{
    UnitResult<Error> Commit();

    UnitResult<Error> Rollback();
}