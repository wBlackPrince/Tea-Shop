namespace Tea_Shop.Contract.Reviews;

public record CreateReviewRequestDto(
    Guid ProductId,
    Guid UserId,
    string Title,
    string Text);