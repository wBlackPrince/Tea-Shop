using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ReviewsEfCoreRepository : IReviewsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ReviewsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Guid> GetReview(Guid tagId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateReview(Review review, CancellationToken cancellationToken)
    {
        await _dbContext.Reviews.AddAsync(review, cancellationToken);

        return review.Id.Value;
    }

    public async Task<Guid> DeleteReview(Guid reviewId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}