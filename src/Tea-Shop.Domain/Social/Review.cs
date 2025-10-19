using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Social;

/// <summary>
/// Domain-модель обзора
/// </summary>
public class Review: Entity
{
    private string _title = string.Empty;
    private string _text = string.Empty;
    private int _rating = 0;

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
        _title = title;
        _text = text;
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
    public string Title => _title;

    /// <summary>
    /// Gets or sets текст обзора
    /// </summary>
    public string Text => _text;

    /// <summary>
    /// Gets or sets рейтинг обзора
    /// </summary>
    public int Rating => _rating;

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

    public UnitResult<Error> UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Error.Validation(
                "update.review",
                "Title can't be empty");
        }

        if (title.Length > Constants.ReviewTitleMaxLength)
        {
            return Error.Validation(
                "update.review",
                "Title can't be longer than 50 characters");
        }

        _title = title;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Error.Validation(
                "update.review",
                "Text can't be empty");
        }

        if (text.Length > Constants.ReviewTextMaxLength)
        {
            return Error.Validation(
                "update.review",
                "Text can't be longer than 2000 characters");
        }

        _text = text;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateRating(int vote)
    {
        if (vote < -1)
        {
            return Error.Validation("review.update", "Rating cannot be below -1");
        }

        if (vote > 1)
        {
            return Error.Validation("review.update", "Rating cannot be abow 1");
        }

        _rating += vote;

        return UnitResult.Success<Error>();
    }
}