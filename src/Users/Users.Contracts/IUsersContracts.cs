using CSharpFunctionalExtensions;
using Shared;
using Shared.Dto;
using Shared.ValueObjects;
using Users.Contracts.Dtos;

namespace Users.Contracts;

public interface IUsersContracts
{
    Task<Result<BasketDto, Error>> CreateBasket(
        CreateBasketRequestDto dto,
        CancellationToken cancellationToken);

    Task<GetUserResponseDto?> GetUserById(
        UserWithOnlyIdDto dto,
        CancellationToken cancellationToken);

    Task<BasketItemDto?> GetBasketItemById(
        BasketItemId basketItemId,
        CancellationToken cancellationToken);

    Task<BasketDto?> GetBasketById(
        BasketId basketId,
        CancellationToken cancellationToken);

    Task<Result<UserId, Error>> UpdateUser(
        UserId userId,
        UpdateEntityRequestDto dto,
        CancellationToken cancellationToken);

    Task<Result<BasketItemId, Error>> UpdateBasketItem(
        BasketItemId basketItemId,
        UpdateEntityRequestDto dto,
        CancellationToken cancellationToken);
}