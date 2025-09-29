using Tea_Shop.Domain.Users;

namespace Tea_Shop.Domain.Tokens;

public class RefreshToken
{
    /// <summary>
    /// ID refresh-token
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Актуальный refresh-token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Id пользователя
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Время истечения токена
    /// </summary>
    public DateTime ExpireOnUtc { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public User User { get; set; }
}