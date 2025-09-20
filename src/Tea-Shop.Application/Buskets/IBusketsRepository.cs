using Tea_Shop.Domain.Buskets;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public interface IBusketsRepository
{
    Task<Guid> Create(Busket busket, CancellationToken cancellationToken);
}