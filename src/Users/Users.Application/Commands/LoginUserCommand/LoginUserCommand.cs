using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.LoginUserCommand;

public record LoginUserCommand(LoginRequestDto Request): ICommand;