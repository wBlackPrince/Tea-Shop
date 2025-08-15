namespace Tea_Shop.Domain.Products;

/// <summary>
/// Domain-модель заказанного элемента
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Initializes a new instance of the "OrderItem" class.
    /// </summary>
    /// <param name="id">Идентификатор заказанного элемента.</param>
    /// <param name="product">Продукт.</param>
    /// <param name="quantity">Количество продукта.</param>
    public OrderItem(Guid id, Product product, int quantity)
    {
        Id = id;
        Product = product;
        Quantity = quantity;
    }

    /// <summary>
    /// Gets or sets Идентификатор заказанного элемента
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets Продукта заказанного элемента
    /// </summary>
    public Product Product { get; set; }

    /// <summary>
    /// Gets or sets Количество заказанного элемента
    /// </summary>
    public int Quantity { get; set; }
}