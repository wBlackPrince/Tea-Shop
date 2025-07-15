namespace TeaShop.Contract.Reviews;

public record CreateReviewDto(
    Guid UserId,
    string Title,
    string Text,
    DateTime CreatedAt,
    DateTime UpdatedAt);