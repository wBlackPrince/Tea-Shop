using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;
using Users.Contracts;
using Users.Contracts.Dtos;
using Users.Domain;

namespace Users.Application.Queries.GetUserByIdQuery;

public class GetUserByIdHandler(
    IUsersReadDbContext readDbContext,
    ILogger<GetUserByIdHandler> logger):
    IQueryHandler<GetUserResponseDto?, GetUserByIdQuery>
{
    public async Task<GetUserResponseDto?> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetUserByIdHandler));

        User? user = await readDbContext.UsersRead.FirstOrDefaultAsync(
            u => u.Id == new UserId(query.Request.UserId),
            cancellationToken);

        if (user is null)
        {
            logger.LogWarning("User with {userId} not found", query.Request.UserId);
            return null;
        }

        var response = new GetUserResponseDto(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Role.ToString(),
            user.BasketId.Value,
            user.AvatarId,
            user.MiddleName);

        logger.LogDebug("Get user with id {userId}.", query.Request.UserId);

        return response;
    }
}