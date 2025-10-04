using Shared.ValueObjects;
using Users.Domain;

namespace Users.Application;

public sealed class EmailVerificationToken
{
    public Guid Id { get; set; }

    public UserId UserId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime ExpiresOnUtc { get; set; }

    public User User { get; set; }
}