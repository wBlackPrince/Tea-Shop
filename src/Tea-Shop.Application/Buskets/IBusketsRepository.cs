using Tea_Shop.Domain.Buskets;

namespace Tea_Shop.Application.Buskets;

public interface IBusketsRepository
{
    Task<Guid> Create(Busket busket, CancellationToken cancellationToken);
}