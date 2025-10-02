namespace Reviews.Contracts.Dtos;

public record CreateReviewResponseDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public int ProductRate { get; init; }

    public string Title { get; init; }

    public string Text { get; init; }
}