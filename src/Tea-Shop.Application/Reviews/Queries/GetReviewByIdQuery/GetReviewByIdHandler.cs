using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Application.Reviews.Queries.GetReviewByIdQuery;

public class GetReviewByIdHandler: IQueryHandler<
    GetReviewResponseDto?,
    GetReviewByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetReviewByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<GetReviewResponseDto?> Handle(
        GetReviewByIdQuery query,
        CancellationToken cancellationToken)
    {
        var review = await _readDbContext.ReviewsRead.FirstOrDefaultAsync(
            r => r.Id == new ReviewId(query.Request.ReviewId),
            cancellationToken);

        if (review is null)
        {
            return null;
        }

        var response = new GetReviewResponseDto()
        {
            Id = review.Id.Value,
            ProductId = review.ProductId.Value,
            UserId = review.UserId.Value,
            Title = review.Title,
            Text = review.Text,
            CreatedAt = review.CreatedAt,
            UpdatedAt = review.UpdatedAt,
        };

        return response;
    }
}