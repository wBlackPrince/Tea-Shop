using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.UpdateUserCommand;

public class UpdateUserHandler
{
    private readonly IUsersRepository _usersRepository;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(
        IUsersRepository usersRepository,
        ILogger<UpdateUserHandler> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }


    public async Task<Result<Guid, Error>> Handle(
        Guid userId,
        JsonPatchDocument<User> userUpdates,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, user, error) = await _usersRepository.GetUser(
            new UserId(userId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        userUpdates.ApplyTo(user);
        await _usersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update user with id {UserId}", userId);

        return user.Id.Value;
    }
}