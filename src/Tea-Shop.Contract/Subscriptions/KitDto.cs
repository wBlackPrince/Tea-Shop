namespace Tea_Shop.Contract.Subscriptions;

public record KitDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public Guid AvatarId { get; init; }

    public string Description { get; init; } = string.Empty;

    public float Sum { get; init; }

    public List<KitItemDto> Items { get; set; } = [];
}