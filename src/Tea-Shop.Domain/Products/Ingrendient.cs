namespace Tea_Shop.Domain.Products;


/// <summary>
/// Класс ингридиента
/// </summary>
public record Ingrendient
{

    /// <summary>
    /// Initializes a new instance of the "Ingridient" class.
    /// </summary>
    /// <param name="amount">Количество.</param>
    /// <param name="name">Название.</param>
    /// <param name="isAllergen">Является ли аллергенным.</param>
    public Ingrendient(
        float amount,
        string name,
        bool isAllergen)
    {
        Amount = amount;
        Name = name;
        IsAllergen = isAllergen;
    }

    /// <summary>
    /// Gets or sets Количество ингридиента
    /// </summary>
    public float Amount { get; }

    /// <summary>
    /// Gets or sets Название ингриндиента
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets Аллергенен ли ингриндиент
    /// </summary>
    public bool IsAllergen { get; }
}