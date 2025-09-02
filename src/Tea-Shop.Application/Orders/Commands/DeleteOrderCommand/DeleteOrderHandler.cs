using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.DeleteOrderCommand;

public class DeleteOrderHandler
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ILogger<DeleteOrderHandler> _logger;

    public DeleteOrderHandler(
        IOrdersRepository ordersRepository,
        ILogger<DeleteOrderHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _ordersRepository
            .DeleteOrder(new OrderId(orderId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error;
        }

        await _ordersRepository.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Deleted order {orderId}", orderId);

        return orderId;
    }
}