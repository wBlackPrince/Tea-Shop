using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Auth;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.LoginUserCommand;

public class LoginUserHandler: ICommandHandler<string, LoginUserCommand>
{
    private readonly ILogger<LoginUserHandler> _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly ITokenProvider _tokenProvider;

    public LoginUserHandler(
        ILogger<LoginUserHandler> logger,
        IUsersRepository usersRepository,
        ITokenProvider tokenProvider)
    {
        _logger = logger;
        _usersRepository = usersRepository;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<string, Error>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handlerName}", nameof(LoginUserHandler));

        var user = await _usersRepository.GetUserByEmail(command.Request.Email, cancellationToken);

        if (user is null)
        {
            _logger.LogError("User not found");
            return Error.NotFound(
                "user.login",
                $"User with email {command.Request.Email} not found");
        }

        if (user.Password != command.Request.Password)
        {
            _logger.LogError("Request's password is incorrect");
            return Error.NotFound(
                "user.login", "Request's password is incorrect");
        }

        string token = _tokenProvider.CreateUser(user);

        return token;
    }
}