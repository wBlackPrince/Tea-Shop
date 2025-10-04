using CSharpFunctionalExtensions;
using Shared;
using Shared.ValueObjects;
using Users.Domain;

namespace Users.Application;

public interface IUsersRepository
{
    Task<User?> GetUserById(
        UserId userId,
        CancellationToken cancellationToken);
    
    Task<BasketItem?> GetBasketItemById(
        BasketItemId basketItemId,
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