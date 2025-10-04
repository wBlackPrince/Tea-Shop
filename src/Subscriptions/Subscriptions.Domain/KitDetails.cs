using Shared.ValueObjects;

namespace Subscriptions.Domain;

/// <summary>
/// Domain-модель детального описания чайного набора
/// </summary>
public class KitDetails
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KitDetails"/> class.
    /// </summary>
    /// <param name="id">Идентификатор деталей набора.</param>
    /// <param name="description">Описание набора.</param>
    /// <param name="sum">Стоимость набора.</param>
    public KitDetails(
        KitDetailsId id,
        string description,
        int sum)
    {
        Id = id;
        Description = description;
        Sum = sum;
    }

    // Для Ef Core.
    private KitDetails()
    {
    }

    /// <summary>
    /// Gets or sets идентификатор деталей подписки.
    /// </summary>
    public KitDetailsId Id { get; set; }

    /// <summary>
    /// Gets or sets описание подписк
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets стоимость всех товаров в подписке
    /// </summary>
    public int Sum { get; set; }
}