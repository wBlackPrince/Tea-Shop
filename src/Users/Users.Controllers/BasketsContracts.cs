using Baskets.Application.Commands.AddBasketCommand;
using Baskets.Contracts;
using Baskets.Contracts.Dtos;
using CSharpFunctionalExtensions;
using Shared;
using Shared.Abstractions;

namespace Baskets.Controllers;

public class BasketsContracts(
    ICommandHandler<BasketDto, CreateBasketCommand> createBasketHandler): IBasketsContracts
{
    public async Task<Result<BasketDto, Error>> CreateBasket(
        CreateBasketRequestDto dto,
        CancellationToken cancellationToken)
    {
        var basket = await createBasketHandler.Handle(new CreateBasketCommand(dto), cancellationToken);
        return basket;
    }
}