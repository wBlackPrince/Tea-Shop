using CSharpFunctionalExtensions;
using Orders.Domain;
using Shared;
using Shared.ValueObjects;

namespace Orders.Application;

public interface IOrdersRepository
{
    Task<Order?> GetOrderById(
        OrderId orderId,
        CancellationToken cancellationToken);

    Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteOrder(OrderId orderId, CancellationToken cancellationToken);
}