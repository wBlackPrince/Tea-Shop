namespace TeaShopDomain.Comments;

/// <summary>
/// Domain-модель для Id
/// </summary>
public class Comment
{

    public Comment(Guid id, Guid userId, Guid reviewId, Guid? parentId, int rate, string text)
    {
        Id = id;
        UserId = userId;
        ReviewId = reviewId;
        ParentId = parentId;
        Rate = rate;
        Text = text;
    }

    /// <summary>
    /// Gets or sets id комментария.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets id пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets id обзора
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Gets or sets рейтинга комментария
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Gets or sets родительского комментария
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Gets or sets идентификаторов дочерних комментариев
    /// </summary>
    public List<Guid> ChildrenIds { get; set; } = [];

    /// <summary>
    /// Gets or sets текста комментария
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets времени создания комментария
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets времени обновления комментария
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}