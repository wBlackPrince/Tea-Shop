using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Commands.RemoveBasketItemCommand;

public record RemoveBasketItemCommand(RemoveBasketItemDto BasketItemDto): ICommand;