using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users;

public interface IUsersService
{
    Task<Guid> CreateUser(
        CreateUserRequestDto request,
        CancellationToken cancellationToken);

    Task<Result<GetUserResponseDto, Error>> Get(
        Guid userId,
        CancellationToken cancellationToken);
}