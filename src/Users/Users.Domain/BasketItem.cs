using Shared.ValueObjects;

namespace Users.Domain;

/// <summary>
/// Domain-модель элемента корзины
/// </summary>
public class BasketItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasketItem"/> class.
    /// </summary>
    /// <param name="id">Идентификатор элемента корзины.</param>
    /// <param name="basketId">Идентификатор корзины.</param>
    /// <param name="productId">Идентификатор продукта.</param>
    /// <param name="quantity">Количество.</param>
    public BasketItem(
        BasketItemId id,
        BasketId basketId,
        ProductId productId,
        int quantity)
    {
        Id = id;
        BasketId = basketId;
        ProductId = productId;
        Quantity = quantity;
    }

    // для Ef Core
    private BasketItem()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор продукта
    /// </summary>
    public BasketItemId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор корзины
    /// </summary>
    public BasketId BasketId { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор продукта
    /// </summary>
    public ProductId ProductId { get; set; }

    /// <summary>
    /// Gets or sets Колиечство.
    /// </summary>
    public int Quantity { get; set; }
}