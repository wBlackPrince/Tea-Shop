using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.UpdateOrderCommand;

public class UpdateOrderHandler
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ILogger<UpdateOrderHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public UpdateOrderHandler(
        IOrdersRepository ordersRepository,
        ILogger<UpdateOrderHandler> logger,
        ITransactionManager transactionManager)
    {
        _ordersRepository = ordersRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid orderId,
        JsonPatchDocument<Order> orderUpdates,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handleName}", nameof(UpdateOrderHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while updating order");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



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



        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while updating order");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        _logger.LogDebug("Update order {orderId}", orderId);

        return order.Id.Value;
    }
}