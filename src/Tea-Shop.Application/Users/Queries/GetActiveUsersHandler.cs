using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Queries;

public class GetActiveUsersHandler
{
    private readonly IUsersRepository _usersRepository;

    public GetActiveUsersHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Result<IReadOnlyList<GetUserResponseDto>, Error>> Handle(
        CancellationToken cancellationToken)
    {
        var (_, isFailure, users, error) = await _usersRepository.GetActiveUsers(cancellationToken);

        if (isFailure)
        {
            return error;
        }

        var usersResponseDto = users.Select(u =>
            new GetUserResponseDto(
                u.Id.Value,
                u.FirstName,
                u.LastName,
                u.Role.ToString(),
                u.AvatarId,
                u.MiddleName)).ToArray();

        return usersResponseDto;
    }
}