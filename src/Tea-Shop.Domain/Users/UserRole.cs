namespace Tea_Shop.Domain.Users;

public sealed class UserRole
{
    /// <summary>
    /// Gets or sets идентифкатор пользователя.
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets or sets Навигационное свойство пользователя.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор роли.
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets Роль пользователя.
    /// </summary>
    public Role Role { get; set; }
}