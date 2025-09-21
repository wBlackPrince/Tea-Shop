using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Baskets;

namespace Tea_Shop.Application.Baskets.Commands.AddBasketItemCommand;

public record AddBasketItemCommand(AddBasketItemDto AddBasketItemDto): ICommand;