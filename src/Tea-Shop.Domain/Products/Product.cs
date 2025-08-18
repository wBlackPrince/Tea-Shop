using Tea_Shop.Domain.Tags;

namespace Tea_Shop.Domain.Products;

/// <summary>
/// Domain-модель продукта
/// </summary>
public class Product
{

    private List<ProductsIngrendients> _ingredients;
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
    /// <param name="rating">Рейтинг.</param>
    /// <param name="ingredients">Список ингриндиентов.</param>
    /// <param name="tagsIds">Список идентификаторов. тегов.</param>
    /// <param name="preparationMethod">Метод приготовления в виде текста.</param>
    /// <param name="photosIds">Список идентификаторов фото.</param>
    public Product(
        Guid id,
        string title,
        string description,
        float price,
        float amount,
        Season season,
        IEnumerable<Ingrendient> ingredients,
        IEnumerable<Guid> tagsIds,
        string preparationMethod,
        IEnumerable<Guid> photosIds)
    {
        Id = id;
        Title = title;
        Description = description;
        Season = season;
        Price = price;
        Amount = amount;

        var productIngredients = ingredients
            .Select(i => new ProductsIngrendients(Guid.NewGuid(), this, i))
            .ToList();

        _ingredients = productIngredients;

        var productsTags = tagsIds
            .Select(tId => new ProductsTags(Guid.NewGuid(), this, tId))
            .ToList();

        _tags = productsTags;

        PreparationMethod = preparationMethod;
        PhotosIds = photosIds.ToArray();
    }

    /// <summary>
    /// Gets or sets идентификатор продукта
    /// </summary>
    public Guid Id { get; set; }

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
    /// Gets or sets список ингридиентов
    /// </summary>
    public IReadOnlyList<ProductsIngrendients> Ingrindients => _ingredients;

    /// <summary>
    /// Gets or sets метод приготовления
    /// </summary>
    public string PreparationMethod { get; set; }

    /// <summary>
    /// Gets or sets список идентификаторов тегов продукта
    /// </summary>
    public IReadOnlyList<ProductsTags> TagsIds => _tags;

    /// <summary>
    /// Gets or sets список идентификаторов фото продукта
    /// </summary>
    public Guid[] PhotosIds { get; set; } = new Guid[0];
}