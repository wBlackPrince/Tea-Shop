using Tea_Shop.Contract.Social;

namespace Tea_Shop.Contract.Users;

public record GetUserCommentsResponseDto
{
    public Guid UserId { get; init; }

    public long TotalCount { get; set; }

    public List<CommentDto> Comments { get; set; } = [];
}
