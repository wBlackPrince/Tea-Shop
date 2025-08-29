using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;


public record OrderItemId(Guid Value);

/// <summary>
/// Domain-модель заказанного элемента
/// </summary>
public class OrderItem
{
    // для Ef Core
    private OrderItem() { }

    /// <summary>
    /// Initializes a new instance of the "OrderItem" class.
    /// </summary>
    /// <param name="id">Идентификатор заказанного элемента.</param>
    /// <param name="product">Продукт.</param>
    /// <param name="quantity">Количество продукта.</param>
    private OrderItem(OrderItemId id, ProductId productId, int quantity)
    {
        Id = id;
        ProductId = productId;
        Quantity = quantity;
    }

    public ProductId ProductId { get; set; }

    public OrderId OrderId { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор заказанного элемента
    /// </summary>
    public OrderItemId Id { get; set; }

    /// <summary>
    /// Gets or sets Количество заказанного элемента
    /// </summary>
    public int Quantity { get; set; }

    public static Result<OrderItem, Error> Create(OrderItemId id, ProductId productId, int quantity)
    {
        if (quantity <= 0)
        {
            return Error.Failure(
                "orderItem.quantity",
                "Order item's quantity must be greater than zero");
        }

        return new OrderItem(id, productId, quantity);
    }
}