using Users.Domain;

namespace Users.Application;

public interface ITokenProvider
{
    string Create(User user);

    string GenerateRefreshToken();
}