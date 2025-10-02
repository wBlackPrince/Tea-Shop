using Orders.Contracts;
using Orders.Contracts.Dtos;
using Shared.Abstractions;

namespace Orders.Application.Commands.DeleteOrderCommand;

public record DeleteOrderCommand(DeleteOrderDto Dto): ICommand;