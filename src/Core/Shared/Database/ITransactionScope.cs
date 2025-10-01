using CSharpFunctionalExtensions;

namespace Shared.Database;

public interface ITransactionScope: IDisposable
{
    UnitResult<Error> Commit();

    UnitResult<Error> Rollback();
}