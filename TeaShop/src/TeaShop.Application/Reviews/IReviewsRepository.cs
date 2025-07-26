using TeaShopDomain.Reviews;

namespace TeaShop.Application.Reviews;

public interface IReviewsRepository
{
    Task<Guid> AddAsync(Review review, CancellationToken cancellationToken);

    Task<Guid> SaveAsync(Review comment, CancellationToken cancellationToken);

    Task<Guid> DeleteAsync(Review commentId, CancellationToken cancellationToken);
}