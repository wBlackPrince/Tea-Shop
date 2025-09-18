using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users;

public interface IUsersRepository
{
    Task<User?> GetUserById(
        UserId userId,
        CancellationToken cancellationToken);

    Task<User?> GetUserByEmail(
        string email,
        CancellationToken cancellationToken);

    Task<bool> IsEmailUnique(
        string email,
        CancellationToken cancellationToken);

    Task<Guid> CreateUser(User user, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteUser(UserId useId, CancellationToken cancellationToken);
}