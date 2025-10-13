using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;


public class UsersRepository(ProductsDbContext dbContext): IUsersRepository
{
    public async Task<User?> GetUserById(
        UserId userId,
        CancellationToken cancellationToken)
    {
        User? user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<User?> GetUserByEmail(
        string email,
        CancellationToken cancellationToken)
    {
        User? user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        return user;
    }

    public async Task<bool> IsEmailUnique(
        string email,
        CancellationToken cancellationToken)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Email == email,
            cancellationToken);

        return user is null;
    }


    public async Task AddUserRole(UserRole userRole, CancellationToken cancellationToken)
    {
        await dbContext.UserRoles.AddAsync(userRole, cancellationToken);
    }

    public async Task<Guid> CreateUser(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);

        return user.Id.Value;
    }

    public async Task<Result<Guid, Error>> DeleteUser(UserId userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken);

        if (user is null)
        {
            return Error.Failure("DeleteUser", "User not found");
        }

        await dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync(cancellationToken);

        return userId.Value;
    }

    public async Task<Basket?> GetBasketById(BasketId basketId, CancellationToken cancellationToken)
    {
        return await dbContext.Baskets.FirstOrDefaultAsync(b => b.Id == basketId, cancellationToken);
    }

    public async Task<BasketItem?> GetBasketItemById(BasketItemId basketItemId, CancellationToken cancellationToken)
    {
        return await dbContext.BasketsItems.FirstOrDefaultAsync(b => b.Id == basketItemId, cancellationToken);
    }

    public async Task<Guid> CreateBasket(Basket basket, CancellationToken cancellationToken)
    {
        await dbContext.Baskets.AddAsync(basket, cancellationToken);

        return basket.Id.Value;
    }
}