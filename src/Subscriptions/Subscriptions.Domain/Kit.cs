using Shared.ValueObjects;

namespace Subscriptions.Domain;

/// <summary>
/// Domain-модель чайного набора
/// </summary>
public class Kit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Kit"/> class.
    /// </summary>
    /// <param name="id">Идентификатор набора.</param>
    /// <param name="name">Имя набора.</param>
    /// <param name="sum">Сумма набора.</param>
    /// <param name="description">Описание набора.</param>
    /// <param name="products">Список продуктов внутри набора.</param>
    public Kit(
        KitId id,
        string name,
        int sum,
        string description,
        IEnumerable<KitItem> products)
    {
        Id = id;
        Name = name;
        Details = new KitDetails(
            new KitDetailsId(Guid.NewGuid()),
            description,
            sum);
        KitItems = products.ToList();
    }

    // Для Ef Core
    private Kit()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор подписки.
    /// </summary>
    public KitId Id { get; set; }

    /// <summary>
    /// Gets or sets Детали подписки.
    /// </summary>
    public KitDetails Details { get; set; }

    /// <summary>
    /// Gets or sets Имя подписки.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets Список продуктов внутри подписки.
    /// </summary>
    public List<KitItem> KitItems { get; set; } = [];

    /// <summary>
    /// Gets or sets Изображение для карточки чайного набора.
    /// </summary>
    public Guid AvatarId { get; set; }
}