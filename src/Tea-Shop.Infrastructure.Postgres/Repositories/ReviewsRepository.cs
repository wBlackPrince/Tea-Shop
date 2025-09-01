using LinqToDB;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ReviewsRepository : IReviewsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ReviewsRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}