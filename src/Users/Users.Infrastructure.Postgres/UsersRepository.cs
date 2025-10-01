namespace Users.Infrastructure.Postgres;


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
        User? user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<User?> GetUserByEmail(
        string email,
        CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

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


    public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);

        return user.Id.Value;
    }

    public async Task<Result<Guid, Error>> DeleteUser(UserId userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken);

        if (user is null)
        {
            return Error.Failure("DeleteUser", "User not found");
        }

        await _dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return userId.Value;
    }
}