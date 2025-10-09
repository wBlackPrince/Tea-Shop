using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Tea_Shop.Application.Auth;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Auth;

public sealed class TokenProvider(
    IConfiguration configuration,
    ProductsDbContext dbContext): ITokenProvider
{
    public async Task<string> Create(User user)
    {
        string secretKeyValue = configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKeyValue));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<string> rolesNames = await dbContext.UserRoles
            .Where(r => r.UserId == user.Id)
            .Select(r => r.Role.Name)
            .ToListAsync();

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("email_verified", user.EmailVerified.ToString()),
        ];

        claims.AddRange(rolesNames.Select(r => new Claim(ClaimTypes.Role, r)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
}