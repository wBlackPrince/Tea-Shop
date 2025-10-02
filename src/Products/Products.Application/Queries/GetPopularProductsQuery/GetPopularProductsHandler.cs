using Dapper;
using Microsoft.Extensions.Logging;
using Products.Contracts.Dtos;
using Shared.Abstractions;
using Shared.Database;

namespace Products.Application.Queries.GetPopularProductsQuery;

public class GetPopularProductsHandler(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<GetPopularProductsHandler> logger):
    IQueryHandler<GetPopularProductsResponseDto[], GetPopularProductsQuery>
{
    public async Task<GetPopularProductsResponseDto[]> Handle(
        GetPopularProductsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetPopularProductsHandler));

        using var connection = await dbConnectionFactory.CreateConnectionAsync(
            cancellationToken: cancellationToken);

        var popularProducts = (await connection.QueryAsync<GetPopularProductsResponseDto>(
            """
            select
                p.id as product_id,
                p.title as product_name,
                sum(oi.quantity) as total_order_quantity
            from products as p 
                inner join order_items as oi on p.id = oi.product_id
                inner join orders as o on o.id = oi.order_id
            WHERE o.created_at > @startSeasonDate and 
                  o.created_at < @endSeasonDate + interval '1' day
            group by p.id, p.title
            order by total_order_quantity DESC
            LIMIT @popularProductsCount
            """,
            param: new
            {
                popularProductsCount = query.Request.PopularProductsCount,
                startSeasonDate = query.Request.StartSeasonDate,
                EndSeasonDate = query.Request.EndSeasonDate,
            })).ToArray();

        if (popularProducts.Length == 0)
        {
            logger.LogWarning("Popular products not found.");
        }

        logger.LogDebug("Get popular products.");

        return popularProducts;
    }
}