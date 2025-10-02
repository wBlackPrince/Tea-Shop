using Baskets.Domain;
using Shared.ValueObjects;

namespace Baskets.Application;

public interface IBasketsRepository
{
    Task<Guid> Create(Basket basket, CancellationToken cancellationToken);

    Task<Basket?> GetById(BasketId basketId, CancellationToken cancellationToken);

    Task<BasketItem?> GetBasketItemById(BasketItemId basketItemId, CancellationToken cancellationToken);
}