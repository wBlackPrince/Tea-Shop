using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;


public record ProductId(Guid Value);

/// <summary>
/// Domain-модель продукта
/// </summary>
public class Product
{
    private List<ProductsTags> _tags;

    // Для Ef Core
    private Product() { }


    /// <summary>
    /// Initializes a new instance of the "Product" class.
    /// </summary>
    /// <param name="id">Идентификатор продукта.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="description">Описание.</param>
    /// <param name="season">Сезон.</param>
    /// /// <param name="price">Цена.</param>
    /// <param name="amount">Количество.</param>
    /// <param name="tagsIds">Список идентификаторов. тегов.</param>
    /// <param name="preparationDescription">Метод приготовления в виде текста.</param>
    /// /// <param name="preparationTime">Время приготовления.</param>
    /// <param name="photosIds">Список идентификаторов фото.</param>
    public Product(
        ProductId id,
        string title,
        string description,
        float price,
        float amount,
        Season season,
        IEnumerable<Ingrendient> ingredients,
        IEnumerable<Guid> tagsIds,
        string preparationDescription,
        int preparationTime,
        IEnumerable<Guid> photosIds)
    {
        Id = id;
        Title = title;
        Description = description;
        Season = season;
        Price = price;
        Amount = amount;

        var productsTags = tagsIds
            .Select(tId => new ProductsTags(
                 new ProductsTagsId(Guid.NewGuid()),
                this,
                new TagId(tId)))
            .ToList();

        _tags = productsTags;

        var productIngredients = ingredients
            .Select(i => new Ingrendient(i.Amount, i.Name, i.Description, i.IsAllergen))
            .ToList();

        PreparationMethod = PreparationMethod.Create(
            preparationTime,
            preparationDescription,
            productIngredients).Value;
        PhotosIds = photosIds.ToArray();
    }

    /// <summary>
    /// Gets or sets идентификатор продукта
    /// </summary>
    public ProductId Id { get; set; }

    /// <summary>
    /// Gets or sets заголовок продукта
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets описание продукта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets сезон данного продукта
    /// </summary>
    public Season Season { get; set; }

    /// <summary>
    /// Gets or sets цену продукта
    /// </summary>
    public float Price { get; set; }

    /// <summary>
    /// Gets or sets количество продукта
    /// </summary>
    public float Amount { get; set; }

    /// <summary>
    /// Gets or sets рейтинг продукта
    /// </summary>
    public int Rating { get; set; } = 0;

    /// <summary>
    /// Gets or sets метод приготовления
    /// </summary>
    public PreparationMethod? PreparationMethod { get; set; }

    /// <summary>
    /// Gets or sets список идентификаторов тегов продукта
    /// </summary>
    public IReadOnlyList<ProductsTags> TagsIds => _tags;

    /// <summary>
    /// Gets or sets список идентификаторов фото продукта
    /// </summary>
    public Guid[] PhotosIds { get; set; }
}