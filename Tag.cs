namespace Tea_Shop.Domain.Tags;


public record TagId(Guid Value);

/// <summary>
/// Domain-модель тега
/// </summary>
public class Tag
{
    // Для Ef Core
    private Tag() { }


    /// <summary>
    /// Initializes a new instance of the "Tag" class.
    /// </summary>
    /// <param name="tagId">Идентфикатор тега.</param>
    /// <param name="name">Название тега.</param>
    /// <param name="description">Описание тега.</param>
    public Tag(TagId tagId, string name, string description)
    {
        Id = tagId;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Gets or sets идентификатор тега
    /// </summary>
    public TagId Id { get; set; }

    /// <summary>
    /// Gets or sets имя тега
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets описание тега
    /// </summary>
    public string Description { get; set; }
}