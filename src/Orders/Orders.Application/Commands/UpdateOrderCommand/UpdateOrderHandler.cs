using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Orders.Domain;
using Shared;
using Shared.Database;
using Shared.ValueObjects;

namespace Orders.Application.Commands.UpdateOrderCommand;

public class UpdateOrderHandler(
    IOrdersRepository ordersRepository,
    ILogger<UpdateOrderHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid orderId,
        JsonPatchDocument<Order> orderUpdates,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handleName}", nameof(UpdateOrderHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while updating order");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        Order? order = await ordersRepository.GetOrderById(
            new OrderId(orderId),
            cancellationToken);

        if (order is null)
        {
            logger.LogError("Not found order with id {orderId}", orderId);
            return Error.NotFound("update order", "order not found");
        }

        try
        {
            orderUpdates.ApplyTo(order);
        }
        catch (Exception e)
        {
            logger.LogError("Validation error while updating order with id {orderId}", orderId);
            return Error.Validation("update.order", e.Message);
        }

        order.UpdatedAt = DateTime.UtcNow.ToUniversalTime();



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating order");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogDebug("Update order {orderId}", orderId);

        return order.Id.Value;
    }
}