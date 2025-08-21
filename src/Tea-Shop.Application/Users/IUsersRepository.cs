using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public interface IUsersRepository
{
    Task<Guid> GetUser(Guid userId, CancellationToken cancellationToken);
    Task<Guid> CreateUser(User user, CancellationToken cancellationToken);
    Task<Guid> DeleteUser(Guid tagId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}