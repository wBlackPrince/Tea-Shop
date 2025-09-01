using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Queries;

public class GetOrderByIdHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetOrderByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<GetOrderResponseDto?> Handle(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        // var (_, isFailure, order, error) = await _ordersRepository.GetOrderById(
        //     new OrderId(orderId),
        //     cancellationToken);

        Order? order = await _readDbContext.OrdersRead
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(
                o => o.Id == new OrderId(orderId),
                cancellationToken);

        if (order is null)
        {
            return null;
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