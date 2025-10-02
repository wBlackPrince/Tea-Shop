using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.CreateUserCommand;

public record CreateUserCommand(CreateUserRequestDto Request): ICommand;