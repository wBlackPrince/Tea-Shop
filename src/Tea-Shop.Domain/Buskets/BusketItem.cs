using Tea_Shop.Domain.Products;

namespace Tea_Shop.Domain.Buskets;

/// <summary>
/// Domain-модель элемента корзины
/// </summary>
public class BusketItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusketItem"/> class.
    /// </summary>
    /// <param name="id">Идентификатор элемента корзины.</param>
    /// <param name="busketId">Идентификатор корзины.</param>
    /// <param name="productId">Идентификатор продукта.</param>
    /// <param name="quantity">Количество.</param>
    public BusketItem(
        BusketItemId id,
        BusketId busketId,
        ProductId productId,
        int quantity)
    {
        Id = id;
        BusketId = busketId;
        ProductId = productId;
        Quantity = quantity;
    }

    // для Ef Core
    private BusketItem()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор продукта
    /// </summary>
    public BusketItemId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор корзины
    /// </summary>
    public BusketId BusketId { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор продукта
    /// </summary>
    public ProductId ProductId { get; set; }

    /// <summary>
    /// Gets or sets Колиечство.
    /// </summary>
    public int Quantity { get; set; }
}