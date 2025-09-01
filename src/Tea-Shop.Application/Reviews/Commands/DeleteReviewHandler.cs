using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands;

public class DeleteReviewHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly IReviewsRepository _reviewsRepository;

    public DeleteReviewHandler(
        IReadDbContext readDbContext,
        IReviewsRepository reviewsRepository)
    {
        _readDbContext = readDbContext;
        _reviewsRepository = reviewsRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var review = await _readDbContext.ReviewsRead
            .FirstOrDefaultAsync(
                r => r.Id == new ReviewId(reviewId),
                cancellationToken);

        if (review is null)
        {
            return Error.NotFound("delete.review", "review not found");
        }

        await _reviewsRepository.DeleteReview(
            new ReviewId(reviewId),
            cancellationToken);

        return review.Id.Value;
    }
}