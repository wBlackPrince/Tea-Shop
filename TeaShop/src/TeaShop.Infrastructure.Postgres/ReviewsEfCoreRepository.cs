using TeaShop.Application.Reviews;
using TeaShopDomain.Reviews;

namespace TeaShop.Infrastructure.Postgres;

public class ReviewsEfCoreRepository: IReviewsRepository
{
    public async Task<Guid> AddAsync(Review review, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> SaveAsync(Review comment, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> DeleteAsync(Review commentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}