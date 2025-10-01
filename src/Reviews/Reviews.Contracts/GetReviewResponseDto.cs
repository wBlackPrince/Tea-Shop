namespace Reviews.Contracts;

public record GetReviewResponseDto
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public Guid UserId { get; init; }

    public int ProductRate { get; init; }

    public string Title { get; init; }

    public string Text { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}