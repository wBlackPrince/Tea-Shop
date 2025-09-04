using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Orders.Queries.GetOrderItemsQuery;

public class GetOrderItemsHandler: IQueryHandler<
    OrderItemDto[], GetOrderItemQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<GetOrderItemsHandler> _logger;

    public GetOrderItemsHandler(
        IDbConnectionFactory connectionFactory,
        ILogger<GetOrderItemsHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<OrderItemDto[]> Handle(
        GetOrderItemQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var orderItems = await connection.QueryAsync<OrderItemDto>(
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
            });

        return orderItems.ToArray();
    }
}