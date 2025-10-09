using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Auth;

public interface ITokenProvider
{
    Task<string> Create(User user);

    string GenerateRefreshToken();
}