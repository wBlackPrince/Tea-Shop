using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Auth;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.LoginUserCommand;

public class LoginUserHandler: ICommandHandler<LoginResponseDto, LoginUserCommand>
{
    private readonly ILogger<LoginUserHandler> _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly ITokensRepository _tokenRepository;
    private readonly ITokenProvider _tokenProvider;
    private readonly ITransactionManager _transactionManager;

    public LoginUserHandler(
        ILogger<LoginUserHandler> logger,
        IUsersRepository usersRepository,
        ITokensRepository tokenRepository,
        ITokenProvider tokenProvider,
        ITransactionManager transactionManager)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _tokenProvider = tokenProvider;
        _tokenRepository = tokenRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<LoginResponseDto, Error>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handlerName}", nameof(LoginUserHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while loging user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var user = await _usersRepository.GetUserByEmail(command.Request.Email, cancellationToken);

        if (user is null)
        {
            _logger.LogError("User not found");
            transactionScope.Rollback();
            return Error.NotFound(
                "user.login",
                $"User with email {command.Request.Email} not found");
        }

        if (user.Password != command.Request.Password)
        {
            _logger.LogError("Request's password is incorrect");
            transactionScope.Rollback();
            return Error.NotFound(
                "user.login", "Request's password is incorrect");
        }

        string accessToken = _tokenProvider.Create(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = _tokenProvider.GenerateRefreshToken(),
            UserId = user.Id,
            ExpireOnUtc = DateTime.UtcNow.AddDays(7),
        };

        await _tokenRepository.CreateRefreshToken(refreshToken, cancellationToken);

        var response = new LoginResponseDto(accessToken, refreshToken.Token);



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

        return response;
    }
}