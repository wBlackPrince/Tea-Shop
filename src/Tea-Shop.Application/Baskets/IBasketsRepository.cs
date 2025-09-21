using Tea_Shop.Domain.Baskets;

namespace Tea_Shop.Application.Baskets;

public interface IBasketsRepository
{
    Task<Guid> Create(Basket basket, CancellationToken cancellationToken);

    Task<Basket?> GetById(BasketId basketId, CancellationToken cancellationToken);

    Task<BasketItem?> GetBasketItemById(BasketItemId basketItemId, CancellationToken cancellationToken);
}