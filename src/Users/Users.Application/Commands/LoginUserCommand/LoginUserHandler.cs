using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.LoginUserCommand;

public class LoginUserHandler(
    ILogger<LoginUserHandler> logger,
    IUsersRepository usersRepository,
    ITokensRepository tokenRepository,
    ITokenProvider tokenProvider,
    ITransactionManager transactionManager,
    IPasswordHasher passwordHasher): ICommandHandler<LoginResponseDto, LoginUserCommand>
{
    public async Task<Result<LoginResponseDto, Error>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handlerName}", nameof(LoginUserHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while loging user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var user = await usersRepository.GetUserByEmail(command.Request.Email, cancellationToken);

        if (user is null || !user.EmailVerified)
        {
            logger.LogError("User not found");
            transactionScope.Rollback();
            return Error.NotFound(
                "user.login",
                $"User with email {command.Request.Email} not found");
        }

        // верификация пароля
        bool verificied = passwordHasher.Verify(command.Request.Password, user.Password);

        if (!verificied)
        {
            logger.LogError("Request's password is incorrect");
            transactionScope.Rollback();
            return Error.NotFound(
                "user.login", "Request's password is incorrect");
        }

        string accessToken = tokenProvider.Create(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = tokenProvider.GenerateRefreshToken(),
            UserId = user.Id,
            ExpireOnUtc = DateTime.UtcNow.AddDays(7),
        };

        await tokenRepository.CreateRefreshToken(refreshToken, cancellationToken);

        var response = new LoginResponseDto(accessToken, refreshToken.Token);



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

        return response;
    }
}