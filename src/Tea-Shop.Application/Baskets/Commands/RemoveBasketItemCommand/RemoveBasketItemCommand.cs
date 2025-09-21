using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Baskets;

namespace Tea_Shop.Application.Baskets.Commands.RemoveBasketItemCommand;

public record RemoveBasketItemCommand(RemoveBasketItemDto BasketItemDto): ICommand;