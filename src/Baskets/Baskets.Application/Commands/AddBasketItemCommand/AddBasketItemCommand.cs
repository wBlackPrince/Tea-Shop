using Baskets.Contracts;
using Baskets.Contracts.Dtos;
using Shared.Abstractions;

namespace Baskets.Application.Commands.AddBasketItemCommand;

public record AddBasketItemCommand(AddBasketItemDto AddBasketItemDto): ICommand;