namespace Tea_Shop.Domain.Products;

/// <summary>
/// Domain-модель ингридиента
/// </summary>
public class Ingrendient
{
    // Для Ef Core
    private Ingrendient() { }

    /// <summary>
    /// Initializes a new instance of the "Ingridient" class.
    /// </summary>
    /// <param name="id">Идентфиикатор ингридиента.</param>
    /// <param name="amount">Количество.</param>
    /// <param name="name">Название.</param>
    /// <param name="description">Описание.</param>
    /// <param name="isAllergen">Является ли аллергенным.</param>
    public Ingrendient(Guid id, float amount, string name, string description, bool isAllergen)
    {
        Id = id;
        Amount = amount;
        Name = name;
        Description = description;
        IsAllergen = isAllergen;
    }

    /// <summary>
    /// Gets or sets Идентификатор ингриндиента
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets Количество ингридиента
    /// </summary>
    public float Amount { get; set; }

    /// <summary>
    /// Gets or sets Название ингриндиента
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets Описание ингриндиента
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets Аллергенен ли ингриндиент
    /// </summary>
    public bool IsAllergen { get; set; }
}