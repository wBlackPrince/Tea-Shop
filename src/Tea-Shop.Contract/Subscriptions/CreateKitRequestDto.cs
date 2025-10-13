namespace Tea_Shop.Contract.Subscriptions;

public record CreateKitRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public CreateKitItemRequestDto[] Items { get; set; } = [];
}