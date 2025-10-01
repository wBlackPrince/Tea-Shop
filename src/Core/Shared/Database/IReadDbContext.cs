namespace Shared.Database;

public interface IReadDbContext
{
    IQueryable<Product> ProductsRead { get; }

    IQueryable<Order> OrdersRead { get; }

    IQueryable<Comment> CommentsRead { get; }

    IQueryable<Review> ReviewsRead { get; }

    IQueryable<User> UsersRead { get; }

    IQueryable<Basket> BusketsRead { get; }

    IQueryable<BasketItem> BusketsItemsRead { get; }
}