namespace Tea_Shop.Contract.Users;

public record BasketDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public IReadOnlyList<BasketItemDto> Items { get; init; } = [];
}