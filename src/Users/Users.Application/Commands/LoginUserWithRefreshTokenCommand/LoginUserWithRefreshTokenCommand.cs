using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.LoginUserWithRefreshTokenCommand;

public record LoginUserWithRefreshTokenCommand(LoginWithRefreshTokenRequestDto Request): ICommand;