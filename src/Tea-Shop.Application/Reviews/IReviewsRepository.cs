using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Application.Reviews;

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