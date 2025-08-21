namespace Tea_Shop.Contract.Comments;

public record CreateCommentRequestDto(
    Guid UserId,
    Guid ReviewId,
    string Text,
    Guid? ParentId);