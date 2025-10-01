using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Commands.LoginUserWithRefreshTokenCommand;

public record LoginUserWithRefreshTokenCommand(LoginWithRefreshTokenRequestDto Request): ICommand;