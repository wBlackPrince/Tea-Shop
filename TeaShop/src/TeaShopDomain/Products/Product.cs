namespace TeaShopDomain.Products;

public class Product
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required float Price { get; set; }

    public required int Amount { get; set; }

    public int? Rating { get; set; }

    public required string Description { get; set; }

    public List<string> Ingredients { get; set; } = [];

    public string Allergens { get; set; } = string.Empty;

    public string PreparationMethod { get; set; } = string.Empty;

    public List<Guid> TagsIds { get; set; } = [];

    public List<Guid> PhotosIds { get; set; } = [];
}