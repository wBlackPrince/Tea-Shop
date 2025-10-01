namespace Shared.Auth;

public interface ITokenProvider
{
    string Create(User user);

    string GenerateRefreshToken();
}