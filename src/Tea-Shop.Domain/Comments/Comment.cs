namespace Tea_Shop.Domain.Comments;

/// <summary>
/// Domain-модель комментария
/// </summary>
public class Comment
{
    // Для Ef Core
    private Comment() {}


    /// <summary>
    /// Initializes a new instance of the "Comment" class.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="text">Текст комментраия.</param>
    /// <param name="rating">Рейтинг комменатрия.</param>
    /// <param name="parentId">Идентификатор родительского комменатрия.</param>
    /// <param name="createdAt">Дата создания.</param>
    /// <param name="updatedAt">Дата обновления.</param>
    public Comment(
        Guid id,
        Guid userId,
        string text,
        DateTime createdAt,
        DateTime updatedAt,
        Guid? parentId = null)
    {
        Id = id;
        ParentId = parentId;
        UserId = userId;
        Text = text;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Gets or sets идентификатор комментария
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets заголовок комментария
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets текст комментария
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Get or sets рейтинг комментария
    /// </summary>
    public int Rating { get; set; } = 0;

    /// <summary>
    /// Get or sets идентификатор родительского комментария
    /// </summary>
    public Guid? ParentId { get; set; } = null;

    /// <summary>
    /// Get or sets список идентификаторов дочерних комменатриев
    /// </summary>
    public Guid[] childrenIds { get; set; } = new Guid[0];

    /// <summary>
    /// Get or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Get or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}