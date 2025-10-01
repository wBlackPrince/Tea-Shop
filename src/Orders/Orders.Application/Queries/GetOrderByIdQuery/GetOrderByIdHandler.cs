using Microsoft.Extensions.Logging;
using Orders.Contracts;
using Shared.Abstractions;
using Shared.Database;

namespace Orders.Application.Queries.GetOrderByIdQuery;

public class GetOrderByIdHandler(
    IDbConnectionFactory connectionFactory,
    ILogger<GetOrderByIdHandler> logger):
    IQueryHandler<GetOrderResponseDto?, GetOrderByIdQuery>
{
    public async Task<GetOrderResponseDto?> Handle(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetOrderByIdHandler));

        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        GetOrderResponseDto? orderDto = null;

        await connection.QueryAsync<GetOrderResponseDto, OrderItemResponseDto, GetOrderResponseDto>(
            """
            select
                o.id,
                o.user_id,
                o.delivery_address,
                o.payment_way,
                o.expected_delivery_time,
                o.order_status,
                o.created_at,
                o.updated_at,
                oi.product_id,
                oi.quantity
            from orders as o
                left join order_items as oi on o.id = oi.order_id
            where o.id = @orderId
            """,
            param: new
            {
                orderId = query.Request.OrderId,
            },
            splitOn: "product_id",
            map: (o, oi) =>
            {
                if (orderDto is null)
                {
                    orderDto = o;
                }

                orderDto.OrderItems.Add(oi);

                return orderDto;
            });

        if (orderDto is null)
        {
            logger.LogWarning("Order not found");
        }

        logger.LogDebug("Order with id {orderId} found.", query.Request.OrderId);

        return orderDto;
    }
}