using Orders.Domain;

namespace Orders.Application;

public interface IOrdersReadDbContext
{
    public IQueryable<Order> OrdersRead { get; }
}