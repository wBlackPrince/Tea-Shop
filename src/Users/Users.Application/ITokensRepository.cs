using Shared.ValueObjects;

namespace Users.Application;

public interface ITokensRepository
{
    Task<RefreshToken?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken);

    Task<EmailVerificationToken?> GetVerificationToken(Guid tokenId, CancellationToken cancellationToken);

    Task CreateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);

    Task CreateVerificationTokenToken(
        EmailVerificationToken verificationToken,
        CancellationToken cancellationToken);

    Task RevokeRefreshTokens(UserId userId, CancellationToken cancellationToken);

    void RemoveVerificationToken(EmailVerificationToken verificationToken);
}