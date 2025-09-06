using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Contract.Reviews;

public record GetReviewCommentsResponseDto
{
    public Guid ReviewId { get; init; }

    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
}