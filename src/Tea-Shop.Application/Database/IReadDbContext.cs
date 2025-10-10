using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Database;

public interface IReadDbContext
{
    IQueryable<Product> ProductsRead { get; }

    IQueryable<Order> OrdersRead { get; }

    IQueryable<Comment> CommentsRead { get; }

    IQueryable<Review> ReviewsRead { get; }

    IQueryable<User> UsersRead { get; }

    IQueryable<Basket> BasketsRead { get; }

    IQueryable<BasketItem> BasketsItemsRead { get; }
}