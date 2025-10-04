using Users.Domain;

namespace Users.Application;

public interface IUsersReadDbContext
{
    public IQueryable<User> UsersRead { get; }
    
    public IQueryable<Basket> BasketsRead { get; }
    
    public IQueryable<BasketItem> BasketsItemsRead { get; }
}