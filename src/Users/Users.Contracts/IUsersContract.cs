namespace Users.Contracts;

public interface IUsersContract
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