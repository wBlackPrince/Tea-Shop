using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Auth;
using Tea_Shop.Domain;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class TokensRepository : ITokensRepository
{
    private readonly ProductsDbContext _context;

    public TokensRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == refreshToken, cancellationToken);
    }

    public async Task CreateRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var c1 = _context.Entry(refreshToken);
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        var c2 = _context.Entry(refreshToken);
    }

    public async Task RevokeRefreshTokens(UserId userId, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens
            .Where(t => t.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}