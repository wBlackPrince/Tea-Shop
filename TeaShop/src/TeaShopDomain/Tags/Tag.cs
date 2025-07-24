namespace TeaShopDomain.Tags;

/// <summary>
/// Domain-модель тега
/// </summary>
public class Tag
{
    public Tag(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Gets or sets идентификатора тега
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets идентификатора имени тега
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets описания тега
    /// </summary>
    public required string Description { get; set; }
}