using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Commands.LoginUserWithRefreshTokenCommand;

public record LoginUserWithRefreshTokenCommand(LoginWithRefreshTokenRequestDto Request): ICommand;