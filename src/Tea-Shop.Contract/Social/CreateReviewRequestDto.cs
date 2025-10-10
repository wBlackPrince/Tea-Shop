namespace Tea_Shop.Contract.Social;

public record CreateReviewRequestDto(
    Guid ProductId,
    Guid UserId,
    int ProductRate,
    string Title,
    string Text);