using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;


public class OrdersRepository: IOrdersRepository
{
    private readonly ProductsDbContext _dbContext;

    public OrdersRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Order?> GetOrderById(OrderId orderId, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);

        return order.Id.Value;
    }

    public async Task<Result<Guid, Error>> DeleteOrder(OrderId orderId, CancellationToken cancellationToken)
    {
        var getOrder = await _dbContext.Orders.FirstOrDefaultAsync(
            o => o.Id == orderId,
            cancellationToken);

        if (getOrder is null)
        {
            return Error.NotFound("delete.order_by_id", "Order not found");
        }

        await _dbContext.Orders
            .Where(o => o.Id == orderId)
            .ExecuteDeleteAsync(cancellationToken);

        return orderId.Value;
    }
}