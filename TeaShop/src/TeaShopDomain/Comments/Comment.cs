namespace TeaShopDomain.Comments;

public class Comment
{
    public Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public required Guid ReviewId { get; set; }

    public required int Rate { get; set; }

    public Comment? Parent { get; set; }

    public List<Guid> ChildrenIds { get; set; } = [];

    public string Text { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}