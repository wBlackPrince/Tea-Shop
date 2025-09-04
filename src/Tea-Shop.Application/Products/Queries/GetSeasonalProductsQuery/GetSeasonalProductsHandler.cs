using Dapper;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetSeasonalProductsQuery;

public class GetSeasonalProductsHandler: IQueryHandler<
    GetSimpleProductResponseDto[],
    GetSeasonalProductsQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetSeasonalProductsHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<GetSimpleProductResponseDto[]> Handle(
        GetSeasonalProductsQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var seasonalProducts = await connection.QueryAsync<GetSimpleProductResponseDto>(
            """
                select 
                    p.id as product_id,
                    p.title as product_name
                from products as p
                where p.season = @season
                limit @productsLimit
            """,
            param:new
            {
                productsLimit = query.Request.ProductsLimit,
                season = query.Request.Season.ToString(),
            });

        return seasonalProducts.ToArray();
    }
}