namespace Tea_Shop.Contract.Social;

public record CreateCommentRequestDto(
    Guid UserId,
    Guid ReviewId,
    string Text,
    Guid? ParentId);