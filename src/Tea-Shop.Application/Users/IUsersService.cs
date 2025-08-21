using Tea_Shop.Contract.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public interface IUsersService
{
    Task<Guid> CreateUser(
        CreateUserRequestDto request,
        CancellationToken cancellationToken);
}