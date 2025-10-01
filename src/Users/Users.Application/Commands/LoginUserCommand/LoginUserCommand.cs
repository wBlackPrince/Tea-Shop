using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Commands.LoginUserCommand;

public record LoginUserCommand(LoginRequestDto Request): ICommand;