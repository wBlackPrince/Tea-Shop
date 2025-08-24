using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users;

public interface IUsersRepository
{
    Task<Result<GetUserResponseDto, Error>> GetUser(
        UserId userId,
        CancellationToken cancellationToken);

    Task<Guid> CreateUser(User user, CancellationToken cancellationToken);

    Task<Guid> DeleteUser(Guid tagId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}