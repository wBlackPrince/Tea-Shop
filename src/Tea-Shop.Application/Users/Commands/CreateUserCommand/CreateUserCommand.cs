using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Commands.CreateUserCommand;

public record CreateUserCommand(CreateUserRequestDto Request): ICommand;