using System.ComponentModel.DataAnnotations;
using Shared;
using Shared.ValueObjects;

namespace Reviews.Domain;

/// <summary>
/// Domain-модель обзора
/// </summary>
public class Review: Entity
{
    private string _title = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Review"/> class.
    /// </summary>
    /// <param name="id">Идентфикатор обзора.</param>
    /// /// <param name="productId">Идентфикатор продукта.</param>
    /// <param name="userId">Идентфикатор пользователя.</param>
    /// <param name="productRating">Рейтинг продукта в обзоре.</param>
    /// <param name="title">Заголовок обзора.</param>
    /// <param name="text">Текст обзора.</param>
    /// <param name="createdAt">Дата создания.</param>
    /// <param name="updatedAt">Дата обновления.</param>
    public Review(
        ReviewId id,
        ProductId productId,
        UserId userId,
        int productRating,
        string title,
        string text,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        ProductId = productId;
        UserId = userId;
        ProductRating = (ProductRates)productRating;
        Title = title;
        Text = text;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    // Для Ef Core
    private Review()
    {
    }

    /// <summary>
    /// Gets идентификатор обзора
    /// </summary>
    public ReviewId Id { get; init; }

    /// <summary>
    /// Gets идентификатор продукта
    /// </summary>
    public ProductId ProductId { get; init; }

    /// <summary>
    /// Gets идентификатор пользователя
    /// </summary>
    public UserId UserId { get; init; }

    /// <summary>
    /// Gets or sets заголовок обзора
    /// </summary>
    public string Title
    {
        get => _title;
        set => UpdateTitle(value);
    }

    /// <summary>
    /// Gets or sets текст обзора
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets рейтинг обзора
    /// </summary>
    public int Rating { get; set; } = 0;

    /// <summary>
    /// Gets or sets рейтинг обзора
    /// </summary>
    public ProductRates ProductRating { get; set; }

    /// <summary>
    /// Gets or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ValidationException("Title can't be empty");
        }

        if (title.Length > Constants.Limit2000)
        {
            throw new ValidationException("Title can't be longer than 2000 characters");
        }

        _title = title;
    }
}