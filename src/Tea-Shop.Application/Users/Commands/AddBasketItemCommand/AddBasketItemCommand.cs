using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Commands.AddBasketItemCommand;

public record AddBasketItemCommand(AddBasketItemDto AddBasketItemDto): ICommand;