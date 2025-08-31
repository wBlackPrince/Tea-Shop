using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Queries;

public class GetOrderByIdHandler
{
    private readonly IOrdersRepository _ordersRepository;

    public GetOrderByIdHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Result<GetOrderResponseDto, Error>> GetOrderById(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, order, error) = await _ordersRepository.GetOrderById(
            new OrderId(orderId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        OrderItemDto[] order_items = order.OrderItems
            .Select(oi => new OrderItemDto(oi.ProductId.Value, oi.Quantity))
            .ToArray();

        GetOrderResponseDto response = new GetOrderResponseDto(
            order.Id.Value,
            order.UserId.Value,
            order.DeliveryAddress,
            order.PaymentWay.ToString(),
            order.ExpectedDeliveryTime,
            order.OrderStatus.ToString(),
            order_items,
            order.CreatedAt,
            order.CreatedAt);

        return response;
    }
}