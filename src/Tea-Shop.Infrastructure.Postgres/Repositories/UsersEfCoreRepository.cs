using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;


public class UsersEfCoreRepository : IUsersRepository
{
    private readonly ProductsDbContext _dbContext;

    public UsersEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<GetUserResponseDto, Error>> GetUser(
        UserId userId,
        CancellationToken cancellationToken)
    {
        User? user = await _dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken);

        if (user is null)
        {
            return Error.Failure("Get.User", "User not found");
        }

        var response = new GetUserResponseDto(
            user.Password,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Role.ToString(),
            user.AvatarId,
            user.MiddleName);

        return response;
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