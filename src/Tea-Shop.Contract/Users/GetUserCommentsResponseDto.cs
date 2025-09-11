using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Contract.Users;

public record GetUserCommentsResponseDto
{
    public Guid UserId { get; init; }

    public List<CommentDto> Comments { get; set; } = [];
}
