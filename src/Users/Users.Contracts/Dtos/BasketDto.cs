namespace Baskets.Contracts.Dtos;

public record BasketDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public IReadOnlyList<BasketItemDto> Items { get; init; } = [];
}