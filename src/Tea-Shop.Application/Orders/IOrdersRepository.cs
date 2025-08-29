using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application.Orders;

public interface IOrdersRepository
{
    Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}