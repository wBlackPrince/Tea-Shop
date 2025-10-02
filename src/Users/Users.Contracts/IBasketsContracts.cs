using Baskets.Contracts.Dtos;
using CSharpFunctionalExtensions;
using Shared;

namespace Baskets.Contracts;

public interface IBasketsContracts
{
    Task<Result<BasketDto, Error>> CreateBasket(
        CreateBasketRequestDto dto,
        CancellationToken cancellationToken);
}