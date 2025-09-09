using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Products.Queries.GetProductReviews;

public class GetProductReviewsHandler:
    IQueryHandler<GetReviewResponseDto[], GetProductReviewsQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<GetProductReviewsHandler> _logger;

    public GetProductReviewsHandler(
        IDbConnectionFactory connectionFactory,
        ILogger<GetProductReviewsHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<GetReviewResponseDto[]> Handle(
        GetProductReviewsQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetProductReviewsHandler));

        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var reviews = (await connection.QueryAsync<GetReviewResponseDto>(
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
            param: new { productId = query.ProductId })).ToArray();

        if (reviews.Length == 0)
        {
            _logger.LogWarning("Reviews not found");
        }

        _logger.LogDebug("Get reviews");

        return reviews.ToArray();
    }
}