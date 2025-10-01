using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Orders.Contracts;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Orders.Application.Commands.DeleteOrderCommand;

public class DeleteOrderHandler(
    IOrdersRepository ordersRepository,
    ILogger<DeleteOrderHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<DeleteOrderDto, DeleteOrderCommand>
{
    public async Task<Result<DeleteOrderDto, Error>> Handle(
        DeleteOrderCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(DeleteOrderHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while deleting order");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var deleteResult = await ordersRepository
            .DeleteOrder(new OrderId(command.Dto.OrderId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            logger.LogError("Failed to delete order with id {orderId}", command.Dto.OrderId);
            transactionScope.Rollback();
            return deleteResult.Error;
        }

        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while deleting order");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Deleted order {orderId}", command.Dto.OrderId);

        var response = new DeleteOrderDto(command.Dto.OrderId);

        return response;
    }
}