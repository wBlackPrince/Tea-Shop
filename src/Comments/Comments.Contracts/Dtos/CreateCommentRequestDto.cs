namespace Comments.Contracts.Dtos;

public record CreateCommentRequestDto(
    Guid UserId,
    Guid ReviewId,
    string Text,
    Guid? ParentId);