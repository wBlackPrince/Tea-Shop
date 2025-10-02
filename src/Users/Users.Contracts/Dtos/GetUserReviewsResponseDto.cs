using Comments.Contracts.Dtos;

namespace Users.Contracts.Dtos;

public record GetUserReviewsResponseDto
{
    public Guid UserId { get; init; }

    public List<ReviewDto> Reviews { get; init; } = [];
}