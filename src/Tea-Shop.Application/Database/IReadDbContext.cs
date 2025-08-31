using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres;

public interface IReadDbContext
{
    IQueryable<Product> ProductsRead { get; }

    IQueryable<Order> OrdersRead { get; }
}