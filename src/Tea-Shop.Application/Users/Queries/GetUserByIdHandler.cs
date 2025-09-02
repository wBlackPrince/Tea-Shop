using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Queries;

public class GetUserByIdHandler
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByIdHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Result<GetUserResponseDto, Error>> Handle(Guid userId, CancellationToken cancellationToken)
    {
        var getResult = await _usersRepository.GetUser(new UserId(userId), cancellationToken);

        if (getResult.IsFailure)
        {
            return getResult.Error;
        }

        User user = getResult.Value;

        var response = new GetUserResponseDto(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Role.ToString(),
            user.AvatarId,
            user.MiddleName);

        return response;
    }
}