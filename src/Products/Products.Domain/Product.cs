using System.ComponentModel.DataAnnotations;
using Shared;
using Shared.ValueObjects;

namespace Products.Domain;

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
        Title = title;
        Description = description;
        Season = season;
        Price = price;
        Amount = amount;
        StockQuantity = stockQuantity;
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
    /// Gets or sets заголовок продукта
    /// </summary>
    public string Title
    {
        get => _title;
        set => UpdateTitle(value);
    }

    /// <summary>
    /// Gets or sets описание продукта
    /// </summary>
    public string Description
    {
        get => _description;
        set => UpdateDescription(value);
    }

    /// <summary>
    /// Gets or sets сезон данного продукта
    /// </summary>
    public Season Season { get; set; }

    /// <summary>
    /// Gets or sets цену продукта
    /// </summary>
    public float Price
    {
        get => _price;
        set => UpdatePrice(value);
    }

    /// <summary>
    /// Gets or sets количество продукта в граммах
    /// </summary>
    public float Amount
    {
        get => _amount;
        set => UpdateAmount(value);
    }

    /// <summary>
    /// Gets or sets количество продукта на складе
    /// </summary>
    public int StockQuantity
    {
        get => _stockQuantity;
        set => UpdateStockQuantity(value);
    }

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




    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ValidationException("Product title cannot be empty");
        }

        if (!(title.Length >= Constants.Limit2) || !(title.Length <= Constants.Limit100))
        {
            throw new ValidationException("Product title has to be between 2 and 100 characters.");
        }

        _title = title;
    }

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ValidationException("Product description cannot be empty");
        }

        if (!(description.Length >= Constants.Limit2) || !(description.Length <= Constants.Limit2000))
        {
            throw new ValidationException("Product description has to be between 2 and 2000 characters.");
        }

        _description = description;
    }

    public void UpdatePrice(float price)
    {
        if (price <= 0)
        {
            throw new ValidationException("Price must be greater than 0");
        }

        _price = price;
    }

    public void UpdateAmount(float amount)
    {
        if (amount <= 0)
        {
            throw new ValidationException("Amount must be greater than 0");
        }

        _amount = amount;
    }

    public void UpdateStockQuantity(int stockQuantity)
    {
        if (stockQuantity <= 0)
        {
            throw new ValidationException("Stock quantity must be greater than 0");
        }

        _stockQuantity = stockQuantity;
    }

    public void UpdateIngredients(Ingrendient[] ingredients)
    {
        PreparationMethod.Ingredients = ingredients.ToList();
    }
}