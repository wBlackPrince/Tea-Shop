using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Commands.CreateUserCommand;

public record CreateUserCommand(CreateUserRequestDto Request): ICommand;