using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;

public class GetUserOrdersHandler:
    IQueryHandler<GetUserOrdersResponseDto?, GetUserOrdersQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetUserCommentsHandler> _logger;

    public GetUserOrdersHandler(
        IDbConnectionFactory dbConnectionFactory,
        ILogger<GetUserCommentsHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<GetUserOrdersResponseDto?> Handle(
        GetUserOrdersQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetUserOrdersHandler));

        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        GetUserOrdersResponseDto? userOrdersDto = null;

        var userOrders = await connection.QueryAsync<
            GetUserOrdersResponseDto,
            OrderDto,
            GetUserOrdersResponseDto>(
            """
                SELECT
                    u.id AS user_id,
                    count(*) OVER () AS total_orders_count,
                    COALESCE(sum(CASE WHEN o.order_status = 'Canceled' THEN 1 END) OVER (), 0) AS canceled_orders_count,
                    COALESCE(sum(CASE WHEN o.order_status = 'Delivered' THEN 1 END) OVER (), 0) AS delivered_orders_count,
                    COALESCE(sum(CASE WHEN o.order_status not in ('Canceled', 'Delivered') THEN 1 END) OVER (), 0) AS not_finished_orders_count,
                    o.id AS order_id,
                    o.delivery_address AS delivery_address,
                    o.payment_way AS payment_way,
                    o.expected_delivery_time AS expected_delivery_time,
                    o.order_status AS order_status,
                    o.created_at AS created_at,
                    o.updated_at AS updated_at
                FROM users AS u INNER JOIN orders AS o ON u.id = o.user_id
                WHERE u.id = @userId
                ORDER BY o.created_at DESC
                LIMIT @ordersLimit
                OFFSET @ordersOffset
                """,
            param: new
            {
                userId = query.Request.UserDto.UserId,
                ordersLimit = query.Request.Pagination.PageSize,
                ordersOffset = (query.Request.Pagination.Page - 1) * query.Request.Pagination.PageSize,
            },
            splitOn: "order_id",
            map: (u, o) =>
            {
                userOrdersDto ??= u;
                userOrdersDto.Orders.Add(o);

                return userOrdersDto;
            });

        if (userOrdersDto is null)
        {
            _logger.LogWarning("User's orders with id {userId}", query.Request.UserDto.UserId);
        }

        _logger.LogDebug("Get user's orders with id {userId}", query.Request.UserDto.UserId);

        return userOrdersDto;
    }
}