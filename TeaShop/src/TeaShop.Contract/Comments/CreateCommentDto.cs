namespace TeaShop.Contract.Comments;

public record CreateCommentDto(
    Guid UserId,
    Guid ReviewId,
    string Text,
    DateTime CreatedAt,
    DateTime UpdatedAt);