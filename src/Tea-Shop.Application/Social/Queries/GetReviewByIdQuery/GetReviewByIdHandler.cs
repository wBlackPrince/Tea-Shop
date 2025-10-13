using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Social;
using Tea_Shop.Domain.Social;

namespace Tea_Shop.Application.Social.Queries.GetReviewByIdQuery;

public class GetReviewByIdHandler(
    IReadDbContext readDbContext,
    ILogger<GetReviewByIdHandler> logger):
    IQueryHandler<GetReviewResponseDto?, GetReviewByIdQuery>
{
    public async Task<GetReviewResponseDto?> Handle(
        GetReviewByIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetReviewByIdHandler));

        var review = await readDbContext.ReviewsRead.FirstOrDefaultAsync(
            r => r.Id == new ReviewId(query.Request.ReviewId),
            cancellationToken);

        if (review is null)
        {
            logger.LogWarning("Review with id {reviewId} not found", query.Request.ReviewId);
            return null;
        }

        var response = new GetReviewResponseDto()
        {
            Id = review.Id.Value,
            ProductId = review.ProductId.Value,
            UserId = review.UserId.Value,
            ProductRate = (int)review.ProductRating,
            Title = review.Title,
            Text = review.Text,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt,
        };

        logger.LogDebug("Get review with id {reviewId}", query.Request.ReviewId);

        return response;
    }
}