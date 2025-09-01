using System.ComponentModel.DataAnnotations.Schema;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Domain.Comments;


public record CommentId(Guid? Value);

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
    /// <param name="reviewId">Идентификатор обзора.</param>
    /// <param name="rating">Рейтинг комменатрия.</param>
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
    public string Text { get; set; }

    /// <summary>
    /// Get or sets рейтинг комментария
    /// </summary>
    public int Rating { get; set; } = 0;

    /// <summary>
    /// Get or sets идентификатор обзора
    /// </summary>
    public ReviewId ReviewId { get; set; }

    /// <summary>
    /// Get or sets идентификатор родительского комментария
    /// </summary>
    [NotMapped]
    public CommentId? ParentId { get; set; } = null;

    /// <summary>
    /// Get or sets список идентификаторов дочерних комменатриев
    /// </summary>
    [NotMapped]
    public CommentId[] childrenIds { get; set; }

    /// <summary>
    /// Get or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Get or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}