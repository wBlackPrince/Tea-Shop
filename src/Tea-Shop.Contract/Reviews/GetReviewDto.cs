namespace Tea_Shop.Contract.Reviews;

public record GetReviewDto
{
    public Guid Id { get; init; }

    public Guid ProductId { get; init; }

    public Guid UserId { get; init; }

    public string Title { get; init; }

    public string Text { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}