using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.UpdateOrderCommand;

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
        _logger.LogDebug("Handling {handleName}", nameof(UpdateOrderHandler));

        Order? order = await _ordersRepository.GetOrderById(
            new OrderId(orderId),
            cancellationToken);

        if (order is null)
        {
            _logger.LogError("Not found order with id {orderId}", orderId);
            return Error.NotFound("update order", "order not found");
        }

        try
        {
            orderUpdates.ApplyTo(order);
        }
        catch (Exception e)
        {
            _logger.LogError("Validation error while updating order with id {orderId}", orderId);
            return Error.Validation("update.order", e.Message);
        }

        order.UpdatedAt = DateTime.UtcNow.ToUniversalTime();

        await _ordersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Update order {orderId}", orderId);

        return order.Id.Value;
    }
}