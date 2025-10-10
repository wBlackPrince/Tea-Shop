using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;

/// <summary>
/// Domain-модель продукта
/// </summary>
public class Product: Entity
{
    private string _title;
    private string _description;
    private float _price;
    private float _amount;
    private int _stockQuantity;
    private int _rating;

    private List<ProductsTags> _tags;

    /// <summary>
    /// Initializes a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="id">Идентификатор продукта.</param>
    /// <param name="title">Заголовок.</param>
    /// <param name="description">Описание.</param>
    /// <param name="season">Сезон.</param>
    /// /// <param name="price">Цена.</param>
    /// <param name="amount">Количество в граммах.</param>
    /// /// <param name="stockQuantity">Количество товара на складе.</param>
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
        int stockQuantity,
        Season season,
        IEnumerable<Ingrendient> ingredients,
        IEnumerable<Guid> tagsIds,
        string preparationDescription,
        int preparationTime,
        IEnumerable<Guid> photosIds)
    {
        Id = id;
        _title = title;
        _description = description;
        Season = season;
        _price = price;
        _amount = amount;
        _stockQuantity = stockQuantity;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        var productsTags = tagsIds
            .Select(tId => new ProductsTags(
                 new ProductsTagsId(Guid.NewGuid()), this, new TagId(tId)))
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

    // Для Ef Core
    private Product()
    {
    }

    /// <summary>
    /// Gets or sets идентификатор продукта
    /// </summary>
    public ProductId Id { get; set; }

    /// <summary>
    /// Gets заголовок продукта
    /// </summary>
    public string Title => _title;

    /// <summary>
    /// Gets описание продукта
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// Gets or sets сезон данного продукта
    /// </summary>
    public Season Season { get; set; }

    /// <summary>
    /// Gets цену продукта
    /// </summary>
    public float Price => _price;

    /// <summary>
    /// Gets количество продукта в граммах
    /// </summary>
    public float Amount => _amount;

    /// <summary>
    /// Gets количество продукта на складе
    /// </summary>
    public int StockQuantity => _stockQuantity;

    /// <summary>
    /// Gets or sets сумма рейтингов обзоров у продукта
    /// </summary>
    public int SumRatings = 0;

    /// <summary>
    /// Gets or sets количество рейтингов обзоров у продукта
    /// </summary>
    public int CountRatings = 0;

    /// <summary>
    /// Gets or sets время создания продукта
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets время последнего обновления продукта
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets метод приготовления
    /// </summary>
    public PreparationMethod PreparationMethod { get; set; }

    /// <summary>
    /// Gets or sets список идентификаторов тегов продукта
    /// </summary>
    public List<ProductsTags> TagsIds
    {
        get
        {
            return _tags;
        }

        private set
        {
            _tags = value;
        }
    }

    /// <summary>
    /// Gets or sets список идентификаторов фото продукта
    /// </summary>
    public Guid[] PhotosIds { get; set; }




    public UnitResult<Error> UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Error.Validation(
                "update.product",
                "Product title cannot be empty");
        }

        if (!(title.Length >= Constants.Limit2) || !(title.Length <= Constants.Limit100))
        {
            return Error.Validation(
                "update.product",
                "Product title has to be between 2 and 100 characters.");
        }

        _title = title;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return Error.Validation(
                "update.product",
                "Product description cannot be empty");
        }

        if (!(description.Length >= Constants.Limit2) || !(description.Length <= Constants.Limit2000))
        {
            return Error.Validation(
                "update.product",
                "Product description has to be between 2 and 2000 characters.");
        }

        _description = description;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdatePrice(float price)
    {
        if (price <= 0)
        {
            return Error.Validation(
                "update.product",
                "Price must be greater than 0");
        }

        _price = price;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateAmount(float amount)
    {
        if (amount <= 0)
        {
            return Error.Validation(
                "update.product",
                "Amount must be greater than 0");
        }

        _amount = amount;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateStockQuantity(int stockQuantity)
    {
        if (stockQuantity <= 0)
        {
            return Error.Validation(
                "update.product",
                "Stock quantity must be greater than 0");
        }

        _stockQuantity = stockQuantity;

        return UnitResult.Success<Error>();
    }

    public void UpdateIngredients(Ingrendient[] ingredients)
    {
        PreparationMethod.Ingredients = ingredients.ToList();
    }
}