using Tea_Shop.Domain.Users;

namespace Tea_Shop.Domain.Buskets;

/// <summary>
/// Domain-модель корзины
/// </summary>
public class Busket
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Busket"/> class.
    /// </summary>
    /// <param name="id">Идентификатор корзины.</param>
    /// /// <param name="userId">Идентификатор пользователя.</param>
    public Busket(BusketId id, UserId userId)
    {
        Id = id;
        UserId = userId;
    }

    // для EF Core
    private Busket()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор корзины
    /// </summary>
    public BusketId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор пользователя
    /// </summary>
    public UserId UserId { get; set; }
}