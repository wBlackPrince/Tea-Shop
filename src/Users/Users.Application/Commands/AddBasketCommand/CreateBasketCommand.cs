using Baskets.Contracts.Dtos;
using Shared.Abstractions;

namespace Baskets.Application.Commands.AddBasketCommand;

public record CreateBasketCommand(CreateBasketRequestDto Request): ICommand;