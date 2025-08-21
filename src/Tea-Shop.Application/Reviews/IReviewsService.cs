using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Reviews;

public interface IReviewsService
{
    Task<Guid> CreateReview(
        CreateReviewRequestDto request,
        CancellationToken cancellationToken);
}