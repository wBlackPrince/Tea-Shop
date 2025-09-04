using Dapper;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetPopularProductsQuery;

public class GetPopularProductsHandler: IQueryHandler<
    GetPopularProductsResponseDto[],
    GetPopularProductsQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetPopularProductsHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<GetPopularProductsResponseDto[]> Handle(
        GetPopularProductsQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(
            cancellationToken: cancellationToken);

        var popularProducts = await connection.QueryAsync<GetPopularProductsResponseDto>(
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
            });

        return popularProducts.ToArray();
    }
}