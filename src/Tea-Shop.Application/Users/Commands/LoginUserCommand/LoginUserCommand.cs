using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Commands.LoginUserCommand;

public record LoginUserCommand(LoginRequestDto Request): ICommand;