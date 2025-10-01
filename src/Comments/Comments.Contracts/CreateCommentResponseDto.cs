namespace Comments.Contracts;

public record CreateCommentResponseDto
{
    public Guid? Id { get; init; }

    public Guid UserId { get; init; }

    public Guid ReviewId { get; init; }

    public string Text { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public Guid? ParentId { get; init; }
};