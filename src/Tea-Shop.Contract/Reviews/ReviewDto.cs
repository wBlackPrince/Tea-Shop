namespace Tea_Shop.Contract.Reviews;

public record ReviewDto
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public int ProductRating { get; init; }

    public int Rating { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Text { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}