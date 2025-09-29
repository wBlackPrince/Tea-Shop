using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Auth;
using Tea_Shop.Domain;
using Tea_Shop.Domain.Tokens;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class TokensRepository : ITokensRepository
{
    private readonly ProductsDbContext _context;

    public TokensRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetRefreshToken(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
    }

    public async Task<EmailVerificationToken?> GetVerificationToken(
        Guid tokenId,
        CancellationToken cancellationToken)
    {
        return await _context.EmailVerificationTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == tokenId, cancellationToken);
    }

    public async Task CreateRefreshToken(
        RefreshToken refreshToken,
        CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        var c2 = _context.Entry(refreshToken);
    }

    public async Task CreateVerificationTokenToken(
        EmailVerificationToken verificationToken,
        CancellationToken cancellationToken)
    {
        await _context.EmailVerificationTokens.AddAsync(verificationToken, cancellationToken);
        var c2 = _context.Entry(verificationToken);
    }

    public async Task RevokeRefreshTokens(UserId userId, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public void RemoveVerificationToken(EmailVerificationToken verificationToken)
    {
        _context.EmailVerificationTokens.Remove(verificationToken);
    }
}