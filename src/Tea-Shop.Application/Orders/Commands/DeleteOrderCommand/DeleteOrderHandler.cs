using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.DeleteOrderCommand;

public class DeleteOrderHandler: ICommandHandler<
    DeleteOrderDto,
    DeleteOrderCommand>
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

    public async Task<Result<DeleteOrderDto, Error>> Handle(
        DeleteOrderCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteOrderHandler));

        var deleteResult = await _ordersRepository
            .DeleteOrder(new OrderId(command.Dto.OrderId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogError("Failed to delete order with id {orderId}", command.Dto.OrderId);
            return deleteResult.Error;
        }

        await _ordersRepository.SaveChangesAsync(cancellationToken);
        _logger.LogDebug("Deleted order {orderId}", command.Dto.OrderId);

        var response = new DeleteOrderDto(command.Dto.OrderId);

        return response;
    }
}