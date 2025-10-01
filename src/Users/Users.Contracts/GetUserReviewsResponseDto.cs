namespace Users.Contracts;

public record GetUserReviewsResponseDto
{
    public Guid UserId { get; init; }

    public List<ReviewDto> Reviews { get; init; } = [];
}