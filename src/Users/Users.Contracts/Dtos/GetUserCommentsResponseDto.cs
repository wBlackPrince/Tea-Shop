using Comments.Contracts.Dtos;

namespace Users.Contracts.Dtos;

public record GetUserCommentsResponseDto
{
    public Guid UserId { get; init; }

    public long TotalCount { get; set; }

    public List<CommentDto> Comments { get; set; } = [];
}
