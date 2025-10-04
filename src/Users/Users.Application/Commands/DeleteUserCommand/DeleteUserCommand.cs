using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.DeleteUserCommand;

public record DeleteUserCommand(UserWithOnlyIdDto Request): ICommand;