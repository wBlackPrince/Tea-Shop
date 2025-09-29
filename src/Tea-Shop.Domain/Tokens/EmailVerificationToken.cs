using Tea_Shop.Domain.Users;

namespace Tea_Shop.Domain.Tokens;

public sealed class EmailVerificationToken
{
    public Guid Id { get; set; }

    public UserId UserId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime ExpiresOnUtc { get; set; }

    public User User { get; set; }
}