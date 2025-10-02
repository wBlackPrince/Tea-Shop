using Baskets.Contracts.Dtos;
using Baskets.Domain;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.ValueObjects;

namespace Baskets.Application.Commands.AddBasketCommand;

public class CreateBasketHandler(
    IBasketsRepository basketsRepository,
    ILogger<CreateBasketHandler> logger): ICommandHandler<BasketDto, CreateBasketCommand>
{
    public async Task<Result<BasketDto, Error>> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        Basket basket = new Basket(
            new BasketId(command.Request.BasketId),
            new UserId(command.Request.UserId));

        await basketsRepository.Create(basket, cancellationToken);

        return new BasketDto
        {
            Id = basket.Id.Value,
            UserId = basket.UserId.Value,
        };
    }
}