using Tea_Shop.Domain.Products;

namespace Tea_Shop.Domain.Subscriptions;

/// <summary>
/// Domain-модель элемента набора.
/// </summary>
public class KitItem
{
    public KitItem(
        KitItemId id,
        ProductId productId,
        int amount)
    {
        Id = id;
        ProductId = productId;
        Amount = amount;
    }

    // Для ef core
    private KitItem()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор элемента набора.
    /// </summary>
    public KitItemId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор продукта.
    /// </summary>
    public ProductId ProductId { get; set; }

    /// <summary>
    /// Gets or sets Количество элемента набора.
    /// </summary>
    public int Amount { get; set; }
}