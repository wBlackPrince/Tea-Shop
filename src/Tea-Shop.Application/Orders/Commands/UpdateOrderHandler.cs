using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands;

public class UpdateOrderHandler
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ILogger<UpdateOrderHandler> _logger;

    public UpdateOrderHandler(
        IOrdersRepository ordersRepository,
        ILogger<UpdateOrderHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid orderId,
        JsonPatchDocument<Order> orderUpdates,
        CancellationToken cancellationToken)
    {
        Order? order = await _ordersRepository.GetOrderById(
            new OrderId(orderId),
            cancellationToken);

        if (order is null)
        {
            return Error.NotFound("update order", "order not found");
        }

        orderUpdates.ApplyTo(order);

        await _ordersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update order {orderId}", orderId);

        return order.Id.Value;
    }
}