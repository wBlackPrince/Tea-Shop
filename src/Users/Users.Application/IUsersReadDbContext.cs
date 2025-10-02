using Users.Domain;

namespace Users.Application;

public interface IUsersReadDbContext
{
    public IQueryable<User> UsersRead { get; }
}