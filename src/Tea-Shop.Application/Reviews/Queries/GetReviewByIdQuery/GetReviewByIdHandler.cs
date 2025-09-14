using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Application.Reviews.Queries.GetReviewByIdQuery;

public class GetReviewByIdHandler: IQueryHandler<
    GetReviewResponseDto?,
    GetReviewByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetReviewByIdHandler> _logger;

    public GetReviewByIdHandler(
        IReadDbContext readDbContext,
        ILogger<GetReviewByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<GetReviewResponseDto?> Handle(
        GetReviewByIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetReviewByIdHandler));

        var review = await _readDbContext.ReviewsRead.FirstOrDefaultAsync(
            r => r.Id == new ReviewId(query.Request.ReviewId),
            cancellationToken);

        if (review is null)
        {
            _logger.LogWarning("Review with id {reviewId} not found", query.Request.ReviewId);
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

        _logger.LogDebug("Get review with id {reviewId}", query.Request.ReviewId);

        return response;
    }
}