using Dapper;
using Microsoft.Extensions.Logging;
using Orders.Contracts;
using Orders.Contracts.Dtos;
using Shared.Abstractions;
using Shared.Database;

namespace Orders.Application.Queries.GetOrderItemsQuery;

public class GetOrderItemsHandler(
    IDbConnectionFactory connectionFactory,
    ILogger<GetOrderItemsHandler> logger):
    IQueryHandler<OrderItemResponseDto[], GetOrderItemQuery>
{
    public async Task<OrderItemResponseDto[]> Handle(
        GetOrderItemQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetOrderItemsHandler));

        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var orderItems = (await connection.QueryAsync<OrderItemResponseDto>(
            """
            select
                oi.product_id,
                oi.quantity
            from order_items as oi
            where oi.order_id = @orderId
            """,
            param: new
            {
                orderId = query.Request.OrderId,
            })).ToArray();

        if (orderItems.Length == 0)
        {
            logger.LogWarning("Items from order with id {orderId} not found", query.Request.OrderId);
        }

        logger.LogDebug("Items from order with id {orderId} found", query.Request.OrderId);

        return orderItems;
    }
}