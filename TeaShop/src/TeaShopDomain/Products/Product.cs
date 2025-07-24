namespace TeaShopDomain.Products;

/// <summary>
/// Domain-модель продукта магазина
/// </summary>
public class Product
{
    public Product(
        Guid id,
        string title,
        float price,
        int amount,
        string description,
        IEnumerable<Guid> tagIds,
        IEnumerable<Guid> photosIds)
    {
        Id = id;
        Title = title;
        Price = price;
        Amount = amount;
        Description = description;
        TagsIds = tagIds.ToList();
        PhotosIds = photosIds.ToList();
    }

    /// <summary>
    /// Gets or sets идентификатора продукта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets заголовка продукта
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets цены продукта
    /// </summary>
    public float Price { get; set; }

    /// <summary>
    /// Gets or sets количества продукта в граммах
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Gets or sets рейтинга продукта
    /// </summary>
    public int? Rating { get; set; }

    /// <summary>
    /// Gets or sets описания продукта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets списка ингриндиентов
    /// </summary>
    public List<string> Ingredients { get; set; } = [];

    /// <summary>
    /// Gets or sets списка аллергенов
    /// </summary>
    public string Allergens { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets описания приготовления продукта
    /// </summary>
    public string PreparationMethod { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets списка тегов продукта
    /// </summary>
    public List<Guid> TagsIds { get; set; } = [];

    /// <summary>
    /// Gets or sets списка идентификаторов изображений
    /// </summary>
    public List<Guid> PhotosIds { get; set; } = [];
}