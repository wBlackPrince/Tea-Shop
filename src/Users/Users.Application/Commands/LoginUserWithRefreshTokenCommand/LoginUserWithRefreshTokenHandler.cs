using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.LoginUserWithRefreshTokenCommand;

public class LoginUserWithRefreshTokenHandler(
    ITokensRepository tokensRepository,
    ILogger<LoginUserWithRefreshTokenHandler> logger,
    ITokenProvider tokenProvider,
    ITransactionManager transactionManager):
    ICommandHandler<LoginResponseDto, LoginUserWithRefreshTokenCommand>
{
    public async Task<Result<LoginResponseDto, Error>> Handle(
        LoginUserWithRefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while loging user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        RefreshToken? refreshToken = await tokensRepository.GetRefreshToken(
            command.Request.RefreshToken,
            cancellationToken);

        if (refreshToken is null)
        {
            transactionScope.Rollback();
            return Error.NotFound(
                "login.with_refresh_token",
                "Refresh token not found");
        }

        if (refreshToken.ExpireOnUtc < DateTime.UtcNow)
        {
            transactionScope.Rollback();
            return Error.NotFound(
                "login.with_refresh_token",
                "Refresh token is expired");
        }

        string accessToken = tokenProvider.Create(refreshToken.User);
        refreshToken.Token = tokenProvider.GenerateRefreshToken();
        refreshToken.ExpireOnUtc = refreshToken.ExpireOnUtc.AddDays(7);





        var saveResult = await transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
        {
            logger.LogError(saveResult.Error.ToString());
            transactionScope.Rollback();
            return saveResult.Error;
        }

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        var response = new LoginResponseDto(accessToken, refreshToken.Token);

        return response;
    }
}