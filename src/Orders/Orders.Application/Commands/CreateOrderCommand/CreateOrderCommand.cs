using Orders.Contracts;
using Orders.Contracts.Dtos;
using Shared.Abstractions;

namespace Orders.Application.Commands.CreateOrderCommand;

public record CreateOrderCommand(CreateOrderRequestDto Request): ICommand;