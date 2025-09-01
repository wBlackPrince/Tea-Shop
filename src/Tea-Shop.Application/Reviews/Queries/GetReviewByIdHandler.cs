using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Application.Reviews.Queries;

public class GetReviewByIdHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetReviewByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Guid?> Handle(Guid reviewId, CancellationToken cancellationToken)
    {
        var review = await _readDbContext.ReviewsRead.FirstOrDefaultAsync(
            r => r.Id == new ReviewId(reviewId),
            cancellationToken);

        return reviewId;
    }
}