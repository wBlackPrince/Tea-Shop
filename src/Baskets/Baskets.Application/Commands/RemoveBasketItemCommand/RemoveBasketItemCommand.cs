using Baskets.Contracts;
using Baskets.Contracts.Dtos;
using Shared.Abstractions;

namespace Baskets.Application.Commands.RemoveBasketItemCommand;

public record RemoveBasketItemCommand(RemoveBasketItemDto BasketItemDto): ICommand;