namespace Tea_Shop.Contract.Social;

public record GetReviewCommentsResponseDto
{
    public Guid ReviewId { get; init; }

    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
}