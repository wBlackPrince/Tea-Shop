using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;


public class UsersRepository : IUsersRepository
{
    private readonly ProductsDbContext _dbContext;

    public UsersRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserById(
        UserId userId,
        CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken);

        return user;
    }

    public async Task<User?> GetUserByEmail(
        string email,
        CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Email == email,
            cancellationToken);

        return user;
    }

    public async Task<bool> IsEmailUnique(
        string email,
        CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Email == email,
            cancellationToken);

        return user is null;
    }


    public async Task<Result<IReadOnlyList<User>, Error>> GetActiveUsers(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
        {
            return Error.Failure("get.active_users", "users not found");
        }

        return users;
    }

    public async Task<Result<IReadOnlyList<User>, Error>> GetBannedUsers(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .Where(u => !u.IsActive)
            .ToListAsync(cancellationToken);

        if (users.Count == 0)
        {
            return Error.Failure("get.banned_users", "users not found");
        }

        return users;
    }


    public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);

        return user.Id.Value;
    }

    public async Task<Result<Guid, Error>> DeleteUser(UserId userId, CancellationToken cancellationToken)
    {
        var id = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (id is null)
        {
            return Error.Failure("DeleteUser", "User not found");
        }

        await _dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return userId.Value;
    }
}