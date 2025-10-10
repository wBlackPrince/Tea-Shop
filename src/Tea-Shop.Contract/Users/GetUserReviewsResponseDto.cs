using Tea_Shop.Contract.Social;

namespace Tea_Shop.Contract.Users;

public record GetUserReviewsResponseDto
{
    public Guid UserId { get; init; }

    public List<ReviewDto> Reviews { get; init; } = [];
}