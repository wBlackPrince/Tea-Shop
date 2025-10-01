using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Commands.DeleteUserCommand;

public record DeleteUserCommand(UserWithOnlyIdDto Request): ICommand;