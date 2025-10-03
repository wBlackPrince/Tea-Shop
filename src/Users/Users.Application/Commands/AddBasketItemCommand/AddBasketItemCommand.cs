using Shared.Abstractions;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.AddBasketItemCommand;

public record AddBasketItemCommand(AddBasketItemDto AddBasketItemDto): ICommand;