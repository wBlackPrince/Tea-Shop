using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Contract.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders;

public interface IOrdersService
{
    Task<Guid> CreateOrder(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken);

    Task<Result<GetOrderResponseDto, Error>> GetOrderById(
        Guid request,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteOrder(
        Guid orderId,
        CancellationToken cancellationToken);
}
