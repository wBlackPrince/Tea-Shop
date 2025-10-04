using Shared.Abstractions;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.CreateBasketCommand;

public record CreateBasketCommand(CreateBasketRequestDto Request): ICommand;