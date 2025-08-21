using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;


public class UsersEfCoreRepository : IUsersRepository
{
    private readonly ProductsDbContext _dbContext;

    public UsersEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> GetUser(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);

        return user.Id.Value;
    }

    public async Task<Guid> DeleteUser(Guid tagId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}