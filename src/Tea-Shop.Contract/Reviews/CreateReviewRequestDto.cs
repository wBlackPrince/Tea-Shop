namespace Tea_Shop.Contract.Reviews;

public record CreateReviewRequestDto(
    Guid ProductId,
    Guid UserId,
    int ProductRate,
    string Title,
    string Text);