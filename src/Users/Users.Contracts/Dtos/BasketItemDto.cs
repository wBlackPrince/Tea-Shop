namespace Users.Contracts.Dtos;

public record BasketItemDto
{
    public Guid Id { get; init; }

    public Guid BasketId { get; init; }

    public Guid ProductId { get; init; }

    public int Quantity { get; init; }
}