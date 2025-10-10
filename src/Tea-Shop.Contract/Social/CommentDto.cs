namespace Tea_Shop.Contract.Social;

public record CommentDto(
    Guid? Id,
    Guid UserId,
    string Text,
    int Rating,
    Guid ReviewId,
    Guid? ParentId,
    string Path,
    DateTime CreatedAt,
    DateTime UpdatedAt);