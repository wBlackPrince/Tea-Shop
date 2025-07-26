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
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets описания тега
    /// </summary>
    public string Description { get; set; }
}