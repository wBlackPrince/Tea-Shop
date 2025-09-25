using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Baskets;

/// <summary>
/// Domain-модель корзины
/// </summary>
public class Basket
{
    private List<BasketItem> _items = new List<BasketItem>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Basket"/> class.
    /// </summary>
    /// <param name="id">Идентификатор корзины.</param>
    /// /// <param name="entityId">Идентификатор сущности.</param>
    public Basket(BasketId id, UserId entityId)
    {
        Id = id;
        UserId = entityId;
    }

    // для EF Core
    private Basket()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор корзины
    /// </summary>
    public BasketId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор пользователя
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets продукты в корзине
    /// </summary>
    public IReadOnlyList<BasketItem> Items => _items;

    public UnitResult<Error> AddItem(BasketItem item)
    {
        if (_items.Count + 1 > Constants.Limit100)
            return Error.Validation("add.basket_item", "Too many items in basket");

        if (item.Quantity < 1 || item.Quantity > Constants.Limit50)
            return Error.Validation("add.basket_item", "Too many or to low items in basket item");

        _items.Add(item);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> RemoveItem(BasketItem item)
    {
        if (_items.Count - 1 > 0)
            return Error.Validation("remove.basket_item", "Item's count in basket cannot be below 0.");

        _items.Remove(item);

        return UnitResult.Success<Error>();
    }
}