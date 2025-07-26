using TeaShop.Contract.Reviews;
using TeaShopDomain.Reviews;

namespace TeaShop.Application.Reviews;

public interface IReviewsService
{
    Task<Guid> Create(CreateReviewDto request, CancellationToken cancellationToken);
}