namespace Baskets.Application.Commands.RemoveBasketItemCommand;

public record RemoveBasketItemCommand(RemoveBasketItemDto BasketItemDto): ICommand;