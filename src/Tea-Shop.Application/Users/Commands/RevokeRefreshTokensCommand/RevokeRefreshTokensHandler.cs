using System.Security.Claims;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Tea_Shop.Application.Auth;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.RevokeRefreshTokensCommand;

public class RevokeRefreshTokensHandler(
    ITokenProvider _tokenProvider,
    ITokensRepository _tokensRepository,
    IHttpContextAccessor _httpContextAccessor)
{
    public async Task<Result<bool, Error>> Handle(Guid userId, CancellationToken cancellationToken)
    {
        if (userId != GetCurrentUserId())
        {
            return Error.Conflict(
                "revoke.refresh_tokens",
                "Current user id not equal transmitted user id.");
        }

        await _tokensRepository.RevokeRefreshTokens(new UserId(userId), cancellationToken);

        return true;
    }

    private Guid? GetCurrentUserId()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
        userIdString = userIdString?.Substring(17, userIdString.Length - 19);

        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var guid))
        {
            return null;
        }

        return guid;
    }
}