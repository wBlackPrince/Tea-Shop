using Microsoft.EntityFrameworkCore;
using Reviews.Application;
using Reviews.Domain;
using Shared.ValueObjects;

namespace Reviews.Infrastructure.Postgres;

public class ReviewsRepository : IReviewsRepository
{
    private readonly ReviewsDbContext _dbContext;

    public ReviewsRepository(ReviewsDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Review?> GetReviewById(
        ReviewId orderId,
        CancellationToken cancellationToken)
    {
        Review? review = await _dbContext.Reviews.FirstOrDefaultAsync(
            o => o.Id == orderId,
            cancellationToken);

        return review;
    }

    public async Task<Guid> CreateReview(Review review, CancellationToken cancellationToken)
    {
        await _dbContext.Reviews.AddAsync(review, cancellationToken);

        return review.Id.Value;
    }

    public async Task<Guid> DeleteReview(
        ReviewId reviewId,
        CancellationToken cancellationToken)
    {
        await _dbContext.Reviews
            .Where(r => r.Id == reviewId)
            .ExecuteDeleteAsync(cancellationToken);

        return reviewId.Value;
    }
}