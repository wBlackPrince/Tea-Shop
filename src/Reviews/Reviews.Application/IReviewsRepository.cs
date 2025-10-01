using Reviews.Domain;
using Shared.ValueObjects;

namespace Reviews.Application;

public interface IReviewsRepository
{
    Task<Review?> GetReviewById(
        ReviewId orderId,
        CancellationToken cancellationToken);

    Task<Guid> CreateReview(Review review, CancellationToken cancellationToken);

    Task<Guid> DeleteReview(
        ReviewId reviewId,
        CancellationToken cancellationToken);
}