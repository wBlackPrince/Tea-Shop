using Dapper;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Products.Queries.GetProductReviews;

public class GetProductReviewsHandler:
    IQueryHandler<GetReviewDto[], GetProductReviewsQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetProductReviewsHandler(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<GetReviewDto[]> Handle(
        GetProductReviewsQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var reviews = await connection.QueryAsync<GetReviewDto>(
            """
            SELECT 
                r.id,
                r.product_id,
                r.user_id,
                r.title,
                r.text,
                r.rating,
                r.created_at,
                r.updated_at
            FROM products AS p INNER JOIN reviews AS r ON p.id = r.product_id
            WHERE p.id = @productId
            ORDER BY r.rating DESC
            """,
            param: new { productId = query.ProductId });

        return reviews.ToArray();
    }
}