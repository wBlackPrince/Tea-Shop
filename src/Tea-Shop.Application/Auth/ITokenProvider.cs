using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Auth;

public interface ITokenProvider
{
    string CreateUser(User user);
}