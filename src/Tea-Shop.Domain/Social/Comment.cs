using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Social;

/// <summary>
/// Domain-модель комментария
/// </summary>
public class Comment: Entity
{
    private string _text;
    private int _rating;

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
        Identifier identifier,
        string text,
        DateTime createdAt,
        DateTime updatedAt,
        Path path,
        int depth,
        CommentId? parentId = null)
    {
        Id = id;
        ParentId = parentId;
        UserId = userId;
        ReviewId = reviewId;
        Identifier = identifier;
        _text = text;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Path = path;
        Depth = depth;
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
    /// Gets текст комментария
    /// </summary>
    public string Text => _text;

    /// <summary>
    /// Gets or sets иднетификатор пути
    /// </summary>
    public Identifier Identifier { get; set; }

    /// <summary>
    /// Gets рейтинг комментария
    /// </summary>
    public int Rating => _rating;

    /// <summary>
    /// Gets идентификатор обзора
    /// </summary>
    public ReviewId ReviewId { get; init; }

    /// <summary>
    /// Gets or sets идентификатор родительского комментария
    /// </summary>
    public CommentId? ParentId { get; set; } = null;

    /// <summary>
    /// Gets or sets навигационное свойство для родительского комментария
    /// </summary>
    public Comment? ParentComment { get; set; } = null;

    /// <summary>
    /// Gets or sets путь к коменнатрию в дереве
    /// </summary>
    public Path Path { get; set; }

    /// <summary>
    /// Gets or sets глубину в дереве
    /// </summary>
    public int Depth { get; set; } = 0;

    /// <summary>
    /// Gets or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }


    public static Comment CreateParent(
        UserId userId,
        ReviewId reviewId,
        string text,
        DateTime createdAt,
        DateTime updatedAt,
        Identifier identifier,
        CommentId? id = null)
    {
        var path = Path.CreateParent(identifier);

        return new Comment(
            id ?? new CommentId(Guid.NewGuid()),
            userId,
            reviewId,
            identifier,
            text,
            createdAt,
            updatedAt,
            path,
            0,
            null);
    }

    public static Comment CreateChild(
        UserId userId,
        ReviewId reviewId,
        string text,
        DateTime createdAt,
        DateTime updatedAt,
        Identifier identifier,
        Comment parent,
        CommentId? parentId = null,
        CommentId? id = null)
    {
        var path = parent.Path.CreateChild(identifier);

        return new Comment(
            id ?? new CommentId(Guid.NewGuid()),
            userId,
            reviewId,
            identifier,
            text,
            createdAt,
            updatedAt,
            path,
            parent.Depth + 1,
            parentId);
    }

    public UnitResult<Error> UpdateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Error.Validation(
                "comment.update",
                "Text cannot be null or whitespace.");
        }

        if (text.Length > Constants.Limit2000)
        {
            return Error.Validation(
                "comment.update",
                "Text cannot be longer than 200 characters.");
        }

        _text = text;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdateRating(int vote)
    {
        if (vote < -1)
        {
            return Error.Validation("comment.update", "Rating cannot be below -1");
        }

        if (vote > 1)
        {
            return Error.Validation("comment.update", "Rating cannot be abow 1");
        }

        _rating += vote;

        return UnitResult.Success<Error>();
    }
}