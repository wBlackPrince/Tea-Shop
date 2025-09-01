using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands;

public class UpdateReviewHandler
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly ILogger<UpdateReviewHandler> _logger;

    public UpdateReviewHandler(
        IReviewsRepository reviewsRepository,
        ILogger<UpdateReviewHandler> logger)
    {
        _reviewsRepository = reviewsRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid reviewId,
        JsonPatchDocument<Review> reviewUpdates,
        CancellationToken cancellationToken)
    {
        Review? review = await _reviewsRepository.GetReviewById(
            new ReviewId(reviewId),
            cancellationToken);

        if (review is null)
        {
            return Error.NotFound("update review", "review not found");
        }

        reviewUpdates.ApplyTo(review);

        await _reviewsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update review {reviewId}", reviewId);

        return review.Id.Value;
    }
}