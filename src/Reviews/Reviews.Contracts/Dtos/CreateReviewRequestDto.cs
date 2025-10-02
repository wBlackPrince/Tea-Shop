namespace Reviews.Contracts.Dtos;

public record CreateReviewRequestDto(
    Guid ProductId,
    Guid UserId,
    int ProductRate,
    string Title,
    string Text);