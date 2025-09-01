using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders;

public interface IOrdersRepository
{
    Task<Order?> GetOrderById(
        OrderId orderId,
        CancellationToken cancellationToken);

    Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteOrder(OrderId orderId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}