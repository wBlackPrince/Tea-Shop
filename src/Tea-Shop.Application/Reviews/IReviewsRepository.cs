using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public interface IReviewsRepository
{
    Task<Guid> GetReview(Guid tagId, CancellationToken cancellationToken);
    Task<Guid> CreateReview(Review review, CancellationToken cancellationToken);
    Task<Guid> DeleteReview(Guid reviewId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}