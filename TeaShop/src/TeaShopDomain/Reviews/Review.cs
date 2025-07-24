namespace TeaShopDomain.Reviews;

/// <summary>
/// Domain-модель обзора
/// </summary>
public class Review
{
    public Review(Guid id, string title, int rate, Guid userId)
    {
        Id = id;
        Title = title;
        Rate = rate;
        UserId = userId;
    }

    /// <summary>
    /// Gets or sets идентификатора обзора
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets заголовка обзора
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets рейтинга обзора
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Gets or sets идентификатора пользователя, который оставил обзор
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets текста обзора
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets времени создания обзора
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets времени обновления обзора
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}