using Dapper;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Orders.Queries.GetOrderByIdQuery;

public class GetOrderByIdHandler: IQueryHandler<
    GetOrderResponseDto?, GetOrderByIdQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetOrderByIdHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<GetOrderResponseDto?> Handle(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        GetOrderResponseDto? orderDto = null;

        var order = await connection.QueryAsync<GetOrderResponseDto, OrderItemDto, GetOrderResponseDto>(
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

        return orderDto;
    }
}