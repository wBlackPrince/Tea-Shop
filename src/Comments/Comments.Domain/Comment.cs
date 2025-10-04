using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared;
using Shared.ValueObjects;

namespace Comments.Domain;

/// <summary>
/// Domain-модель комментария
/// </summary>
public class Comment: Entity
{
    private string _text;

    /// <summary>
    /// Initializes a new instance of the <see cref="Comment"/> class.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="text">Текст комментраия.</param>
    /// <param name="reviewId">Идентификатор обзора.</param>
    /// <param name="parentId">Идентификатор родительского комменатрия.</param>
    /// <param name="createdAt">Дата создания.</param>
    /// <param name="updatedAt">Дата обновления.</param>
    public Comment(
        CommentId id,
        UserId userId,
        ReviewId reviewId,
        string text,
        DateTime createdAt,
        DateTime updatedAt,
        CommentId? parentId = null)
    {
        Id = id;
        ParentId = parentId;
        UserId = userId;
        ReviewId = reviewId;
        Text = text;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    // Для Ef Core
    private Comment()
    {
    }

    /// <summary>
    /// Gets or sets идентификатор комментария
    /// </summary>
    public CommentId Id { get; set; }

    /// <summary>
    /// Gets or sets идентификатор пользователя
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets or sets текст комментария
    /// </summary>
    public string Text
    {
        get => _text;
        set => UpdateText(value);
    }

    /// <summary>
    /// Gets or sets рейтинг комментария
    /// </summary>
    public int Rating { get; set; } = 0;

    /// <summary>
    /// Gets идентификатор обзора
    /// </summary>
    public ReviewId ReviewId { get; init; }

    /// <summary>
    /// Gets or sets идентификатор родительского комментария
    /// </summary>
    public CommentId? ParentId { get; set; } = null;

    /// <summary>
    /// Gets or sets родительский комментарий
    /// </summary>
    public Comment Parent { get; set; }

    /// <summary>
    /// Gets or sets список идентификаторов дочерних комменатриев
    /// </summary>
    public ICollection<Comment> Children { get; set; } = new List<Comment>();

    /// <summary>
    /// Gets or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public void UpdateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ValidationException("Text cannot be null or empty.");
        }

        if (text.Length > Constants.Limit2000)
        {
            throw new ValidationException("Text cannot be longer than 200 characters.");
        }

        _text = text;
    }
}