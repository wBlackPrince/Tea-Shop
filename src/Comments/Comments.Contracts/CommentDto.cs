namespace Comments.Contracts;

public record CommentDto(
    Guid? Id,
    Guid UserId,
    string Text,
    int Rating,
    Guid ReviewId,
    Guid? ParentId,
    DateTime CreatedAt,
    DateTime UpdatedAt);