using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetSimilarProductsQuery;

public class GetSimilarProductsHandler
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetSimilarProductsHandler> _logger;

    public GetSimilarProductsHandler(
        IDbConnectionFactory dbConnectionFactory,
        ILogger<GetSimilarProductsHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<GetProductsResponseDto> GetSimilarProducts(
        GetSimilarProductsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        var similarProducts = await connection.QueryAsync("""
                                                          with chosen_product as (
                                                              select p.id,
                                                                     p.price,
                                                                     p.season,
                                                                     p.ingredients ->> 'Ingredients' as ingredients,
                                                                     pt.tag_id
                                                              from products as p join products_tags as pt on p.id = pt.product_id
                                                              where p.id = @productId
                                                          )
                                                          
                                                          select DISTINCT p.id,
                                                                 p.title,
                                                                 p.price,
                                                                 p.amount,
                                                                 p.stock_quantity,
                                                                 p.description,
                                                                 p.season
                                                          from products as p 
                                                              join products_tags as pt on pt.product_id = p.id
                                                              join chosen_product as cp
                                                              ON (cp.season = p.season) and 
                                                                 ((round(cp.price / p.price) < 2) or (round(p.price / cp.price) < 2)) and
                                                                 pt.tag_id = cp.tag_id
                                                          limit 10
                                                          """);

        throw new NotImplementedException();
    }
}