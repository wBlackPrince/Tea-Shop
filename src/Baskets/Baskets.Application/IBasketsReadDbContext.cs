using Baskets.Domain;

namespace Baskets.Application;

public interface IBasketsReadDbContext
{
    IQueryable<Basket> BusketsRead { get; }

    IQueryable<BasketItem> BusketsItemsRead { get; }
}