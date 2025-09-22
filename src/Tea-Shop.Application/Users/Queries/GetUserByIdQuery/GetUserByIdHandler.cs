using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserByIdQuery;

public class GetUserByIdHandler:
    IQueryHandler<GetUserResponseDto?, GetUserByIdQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetUserByIdHandler> _logger;

    public GetUserByIdHandler(
        IReadDbContext readDbContext,
        ILogger<GetUserByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<GetUserResponseDto?> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetUserByIdHandler));

        User? user = await _readDbContext.UsersRead.FirstOrDefaultAsync(
            u => u.Id == new UserId(query.Request.UserId),
            cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with {userId} not found", query.Request.UserId);
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

        _logger.LogDebug("Get user with id {userId}.", query.Request.UserId);

        return response;
    }
}