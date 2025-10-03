using Shared.Abstractions;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.RemoveBasketItemCommand;

public record RemoveBasketItemCommand(RemoveBasketItemDto BasketItemDto): ICommand;