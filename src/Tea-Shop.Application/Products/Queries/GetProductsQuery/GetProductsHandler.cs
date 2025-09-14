using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetProductsQuery;

public class GetProductsHandler: IQueryHandler<GetProductsResponseDto, GetProductsQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetProductsHandler> _logger;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetProductsHandler(
        IReadDbContext readDbContext,
        ILogger<GetProductsHandler> logger,
        IDbConnectionFactory dbConnectionFactory)
    {
        _readDbContext = readDbContext;
        _logger = logger;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<GetProductsResponseDto> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetProductsHandler));

        var parameters = new DynamicParameters();
        var conditions = new List<string>();

        if (!string.IsNullOrWhiteSpace(query.Request.Search))
        {
            parameters.Add("search", query.Request.Search, DbType.String);
            conditions.Add("p.title ILIKE '%' || @search || '%'");
        }

        if (!string.IsNullOrWhiteSpace(query.Request.Season))
        {
            parameters.Add("season", query.Request.Season, DbType.String);
            conditions.Add("p.season = @season");
        }

        if (query.Request.TagId is not null)
        {
            parameters.Add("tag_id", query.Request.TagId, DbType.Guid);
            conditions.Add("pt.tag_id = @tag_id");
        }

        if (query.Request.MinPrice is not null)
        {
            parameters.Add("min_price", query.Request.MinPrice, DbType.Int32);
            conditions.Add("p.price >= @min_price");
        }

        if (query.Request.MaxPrice is not null)
        {
            parameters.Add("max_price", query.Request.MaxPrice, DbType.Int32);
            conditions.Add("p.price <= @max_price");
        }

        var whereClause = (conditions.Count > 0) ?
            "WHERE " + string.Join(" AND ", conditions) :
            string.Empty;

        parameters.Add("limit", query.Request.Pagination.PageSize, DbType.Int32);
        parameters.Add(
            "offset",
            (query.Request.Pagination.Page - 1) * query.Request.Pagination.PageSize,
            DbType.Int32);


        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        GetProductsResponseDto? products = new GetProductsResponseDto();
        long totalCount = 0;

        await connection
            .QueryAsync<ProductDto, long, GetProductsResponseDto>(
            $"""
                 SELECT
                    p.id,
                    p.title,
                    p.price,
                    p.amount,
                    p.stock_quantity,
                    p.description,
                    p.season,
                    (SELECT round(avg(r1.product_rating), 2)
                         FROM reviews AS r1
                         WHERE r1.product_id = p.id) AS rating,
                    (SELECT count(*)
                         FROM reviews AS r2
                          WHERE r2.product_id = p.id) AS reviews_count,
                    p.created_at,
                    p.updated_at,
                    array_agg(pt.tag_id) as tags_ids,
                    p.photos_ids as photos_ids,
                    
                    count(*) OVER () AS total_count
                 FROM products AS p
                      JOIN products_tags AS pt ON p.id = pt.product_id
                 {whereClause}
                 GROUP BY p.id, p.title, p.price, p.amount, p.stock_quantity, p.description, p.season, p.created_at, p.updated_at
                 LIMIT @limit
                 OFFSET @offset
                 """,
            param: parameters,
            splitOn: "total_count",
            map: (p, tc) =>
            {
                totalCount = tc;
                products.Products.Add(p);

                return products;
            });

        products.TotalCount = totalCount;
        return products;
    }
}