using System.Data;
using CSharpFunctionalExtensions;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Auth;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.LoginUserCommand;

public class LoginUserHandler(
    ILogger<LoginUserHandler> logger,
    IUsersRepository usersRepository,
    ITokensRepository tokenRepository,
    ITokenProvider tokenProvider,
    ITransactionManager transactionManager,
    IFluentEmail fluentEmail): ICommandHandler<LoginResponseDto, LoginUserCommand>
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

        if (user is null)
        {
            logger.LogError("User not found");
            transactionScope.Rollback();
            return Error.NotFound(
                "user.login",
                $"User with email {command.Request.Email} not found");
        }

        if (user.Password != command.Request.Password)
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


        // email verification
        await fluentEmail
            .To(user.Email)
            .Subject("Email verification for TeaShop")
            .Body("To verify your email address, click here")
            .SendAsync();

        return response;
    }
}