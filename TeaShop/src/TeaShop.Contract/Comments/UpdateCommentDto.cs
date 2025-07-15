namespace TeaShop.Contract.Comments;

public record UpdateCommentDto(
    string Text,
    DateTime UpdatedAt);