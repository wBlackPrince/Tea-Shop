using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;

public class GetUserOrdersHandler:
    IQueryHandler<GetUserOrdersResponse, GetUserOrdersQuery>
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

    public async Task<GetUserOrdersResponse> Handle(
        GetUserOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var userOrders = """
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
                         WHERE u.id = '017ac0d7-7695-4100-a935-4027f723d5e8'
                         """;

        throw new NotImplementedException();
    }
}