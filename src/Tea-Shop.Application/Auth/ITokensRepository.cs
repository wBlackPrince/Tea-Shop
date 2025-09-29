using Tea_Shop.Domain;
using Tea_Shop.Domain.Tokens;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Auth;

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