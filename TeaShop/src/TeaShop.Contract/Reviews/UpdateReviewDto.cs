namespace TeaShop.Contract.Reviews;

public record UpdateReviewDto(
    string Title,
    string Text,
    DateTime UpdatedAt);