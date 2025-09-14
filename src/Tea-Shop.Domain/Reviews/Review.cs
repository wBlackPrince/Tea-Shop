using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Domain.Reviews;


public record ReviewId(Guid Value);

/// <summary>
/// Domain-модель обзора
/// </summary>
public class Review
{
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
    /// Gets or sets идентификатор обзора
    /// </summary>
    public ReviewId Id { get; set; }

    /// <summary>
    /// Gets or sets идентификатор продукта
    /// </summary>
    public ProductId ProductId { get; set; }

    /// <summary>
    /// Gets or sets идентификатор пользователя
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets or sets заголовок обзора
    /// </summary>
    public string Title { get; set; }

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
}