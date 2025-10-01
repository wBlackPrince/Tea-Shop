namespace Comments.Contracts;

public record CreateCommentRequestDto(
    Guid UserId,
    Guid ReviewId,
    string Text,
    Guid? ParentId);