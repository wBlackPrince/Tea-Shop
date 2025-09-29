using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetProductsQuery;

public class GetProductsHandler(
    IReadDbContext readDbContext,
    ILogger<GetProductsHandler> logger,
    IDbConnectionFactory dbConnectionFactory):
    IQueryHandler<GetProductsResponseDto, GetProductsQuery>
{
    public async Task<GetProductsResponseDto> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetProductsHandler));

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

        string orderByField = query.Request?.OrderBy?.ToLower() switch
        {
            "season" => "p.season",
            "price" => "p.price",
            "created_at" => "p.created_at",
            "rating" => "rating",
            _ => "p.created_at",
        };

        string orderByDirection = (query.Request?.OrderDirection?.ToLower() == "asc") ? "ASC" : "DESC";

        var orderClause = $"ORDER BY {orderByField} {orderByDirection}";


        parameters.Add("limit", query.Request?.Pagination.PageSize, DbType.Int32);
        parameters.Add(
            "offset",
            (query.Request?.Pagination.Page - 1) * query.Request?.Pagination.PageSize,
            DbType.Int32);


        var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        GetProductsResponseDto? products = new GetProductsResponseDto();
        long totalCount = 0;

        await connection
            .QueryAsync<ProductDto, long, GetProductsResponseDto>(
            $"""
                 WITH reviews_count as(
                 SELECT 
                     p.id as product_id, 
                     count(*) as reviews_count
                 FROM reviews as r join products as p ON r.product_id = p.id
                 GROUP BY p.id)
                 
                 SELECT
                    p.id,
                    p.title,
                    p.price,
                    p.amount,
                    p.stock_quantity,
                    p.description,
                    p.season,
                    CASE 
                        WHEN p.count_ratings > 0 THEN p.sum_ratings / p.count_ratings
                        ELSE 0 
                    END AS rating,
                    rc.reviews_count,
                    p.created_at,
                    p.updated_at,
                    array_agg(pt.tag_id) as tags_ids,
                    p.photos_ids as photos_ids,
                    
                    count(*) OVER () AS total_count
                 FROM products AS p
                      JOIN products_tags AS pt ON p.id = pt.product_id
                      JOIN reviews_count AS rc ON rc.product_id = p.id
                 {whereClause}
                 GROUP BY p.id, p.title, p.price, p.amount, p.stock_quantity, p.description, p.season, p.created_at, p.updated_at, rc.reviews_count
                 {orderClause}
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