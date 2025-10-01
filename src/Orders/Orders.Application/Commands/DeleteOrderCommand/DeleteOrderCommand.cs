using Orders.Contracts;
using Shared.Abstractions;

namespace Orders.Application.Commands.DeleteOrderCommand;

public record DeleteOrderCommand(DeleteOrderDto Dto): ICommand;