using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Auth;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.LoginUserWithRefreshTokenCommand;

public class LoginUserWithRefreshTokenHandler:
    ICommandHandler<LoginResponseDto, LoginUserWithRefreshTokenCommand>
{
    private readonly ITokensRepository _tokensRepository;
    private readonly ILogger<LoginUserWithRefreshTokenHandler> _logger;
    private readonly ITokenProvider _tokenProvider;
    private readonly ITransactionManager _transactionManager;

    public LoginUserWithRefreshTokenHandler(
        ITokensRepository tokensRepository,
        ILogger<LoginUserWithRefreshTokenHandler> logger,
        ITokenProvider tokenProvider,
        ITransactionManager transactionManager)
    {
        _tokensRepository = tokensRepository;
        _logger = logger;
        _tokenProvider = tokenProvider;
        _transactionManager = transactionManager;
    }

    public async Task<Result<LoginResponseDto, Error>> Handle(
        LoginUserWithRefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while loging user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        RefreshToken? refreshToken = await _tokensRepository.GetRefreshToken(
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

        string accessToken = _tokenProvider.Create(refreshToken.User);
        refreshToken.Token = _tokenProvider.GenerateRefreshToken();
        refreshToken.ExpireOnUtc = refreshToken.ExpireOnUtc.AddDays(7);





        var saveResult = await _transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
        {
            _logger.LogError(saveResult.Error.ToString());
            transactionScope.Rollback();
            return saveResult.Error;
        }

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while creating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        var response = new LoginResponseDto(accessToken, refreshToken.Token);

        return response;
    }
}