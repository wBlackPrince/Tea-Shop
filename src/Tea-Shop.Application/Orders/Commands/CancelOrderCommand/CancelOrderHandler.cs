using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Orders.Commands.CreateOrderCommand;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.CancelOrderCommand;

public class CancelOrderHandler: ICommandHandler<OrderWithOnlyIdDto, CancelOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CancelOrderHandler(
        IOrdersRepository ordersRepository,
        ILogger<CreateOrderHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _logger = logger;
    }

    public async Task<Result<OrderWithOnlyIdDto, Error>> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _ordersRepository.GetOrderById(new OrderId(request.OrderId), cancellationToken);

        if (order is null)
        {
            _logger.LogError("Order with id {OrderId} not found", request.OrderId);
            return Error.NotFound("cancel.order", "order not found");
        }

        var cancelResult = order.UpdateStatus(OrderStatus.Canceled);

        if (cancelResult.IsFailure)
        {
            _logger.LogError("Order with id {OrderId} is already delivered", request.OrderId);
            return cancelResult.Error;
        }

        await _ordersRepository.SaveChangesAsync(cancellationToken);

        return new OrderWithOnlyIdDto(request.OrderId);
    }
}