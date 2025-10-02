using Comments.Contracts.Dtos;
using Dapper;
using Microsoft.Extensions.Logging;
using Shared.Abstractions;
using Shared.Database;

namespace Products.Application.Queries.GetProductReviews;

public class GetProductReviewsHandler(
    IDbConnectionFactory connectionFactory,
    ILogger<GetProductReviewsHandler> logger):
    IQueryHandler<GetReviewResponseDto[], GetProductReviewsQuery>
{
    public async Task<GetReviewResponseDto[]> Handle(
        GetProductReviewsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetProductReviewsHandler));

        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var reviews = (await connection.QueryAsync<GetReviewResponseDto>(
            """
            SELECT 
                r.id,
                r.product_id,
                r.user_id,
                r.product_rating,
                r.title,
                r.text,
                r.rating,
                r.created_at,
                r.updated_at
            FROM products AS p INNER JOIN reviews AS r ON p.id = r.product_id
            WHERE p.id = @productId
            ORDER BY r.rating DESC
            """,
            param: new { productId = query.ProductId })).ToArray();

        if (reviews.Length == 0)
        {
            logger.LogWarning("Reviews not found");
        }

        logger.LogDebug("Get reviews");

        return reviews.ToArray();
    }
}