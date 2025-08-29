using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Orders;

public interface IOrdersService
{
    Task<Guid> CreateOrder(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken);
}
