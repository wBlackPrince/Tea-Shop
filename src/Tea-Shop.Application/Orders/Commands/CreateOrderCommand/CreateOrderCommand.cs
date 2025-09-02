using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Orders.Commands.CreateOrderCommand;

public record CreateOrderCommand(CreateOrderRequestDto Request): ICommand;