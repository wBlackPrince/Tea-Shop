using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
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
    private ITransactionManager _transactionManager;

    public DeleteOrderHandler(
        IOrdersRepository ordersRepository,
        ILogger<DeleteOrderHandler> logger,
        ITransactionManager transactionManager)
    {
        _ordersRepository = ordersRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<DeleteOrderDto, Error>> Handle(
        DeleteOrderCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteOrderHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while deleting order");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var deleteResult = await _ordersRepository
            .DeleteOrder(new OrderId(command.Dto.OrderId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogError("Failed to delete order with id {orderId}", command.Dto.OrderId);
            transactionScope.Rollback();
            return deleteResult.Error;
        }

        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while deleting order");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Deleted order {orderId}", command.Dto.OrderId);

        var response = new DeleteOrderDto(command.Dto.OrderId);

        return response;
    }
}