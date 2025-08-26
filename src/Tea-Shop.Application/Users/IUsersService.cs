using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users;

public interface IUsersService
{
    Task<Guid> CreateUser(
        CreateUserRequestDto request,
        CancellationToken cancellationToken);

    Task<Result<GetUserResponseDto, Error>> GetById(
        Guid userId,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> UpdateUser(
        Guid userId,
        JsonPatchDocument<User> userUpdates,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteUser(
        Guid userId,
        CancellationToken cancellationToken);
}