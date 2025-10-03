using Users.Domain;

namespace Users.Application;

public interface IBasketsReadDbContext
{
    IQueryable<Basket> BusketsRead { get; }

    IQueryable<BasketItem> BusketsItemsRead { get; }
    
    IQueryable<Basket> BasketsRead { get; }
    
    IQueryable<BasketItem> BasketsItemsRead { get; }
}