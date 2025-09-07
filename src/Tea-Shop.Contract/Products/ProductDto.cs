namespace Tea_Shop.Contract.Products;

public record ProductDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public float Price { get; init; }

    public float Amount { get; init; }

    public int StockQuantity { get; init; }

    public string Description { get; init; } = string.Empty;

    public string Season { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public Guid[] TagsIds { get; init; } = [];

    public Guid[] PhotosIds { get; init; } = [];
}