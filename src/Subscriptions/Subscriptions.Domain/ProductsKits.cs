namespace Subscriptions.Domain;

public class ProductsKits
{
    public ProductsKits(
        ProductsKitsId id,
        KitId kitId,
        ProductId productId)
    {
        Id = id;
        KitId = kitId;
        ProductId = productId;
    }

    // Для ef core
    private ProductsKits()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор промежуточной таблицы.
    /// </summary>
    public ProductsKitsId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор чайного набора.
    /// </summary>
    public KitId KitId { get; set; }

    /// <summary>
    /// Gets or sets Идентифкатор продукта.
    /// </summary>
    public ProductId ProductId { get; set; }
}