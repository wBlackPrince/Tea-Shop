using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Orders.Application;
using Orders.Domain;
using Shared;
using Shared.ValueObjects;

namespace Orders.Infrastructure.Postgres;


public class OrdersRepository: IOrdersRepository
{
    private readonly OrdersDbContext _dbContext;

    public OrdersRepository(OrdersDbContext dbContext)
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