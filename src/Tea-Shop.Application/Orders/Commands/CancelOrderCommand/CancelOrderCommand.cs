using Tea_Shop.Application.Abstractions;

namespace Tea_Shop.Application.Orders.Commands.CancelOrderCommand;

public record CancelOrderCommand(Guid OrderId): ICommand;