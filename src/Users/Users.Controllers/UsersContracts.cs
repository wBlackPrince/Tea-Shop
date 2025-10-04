using CSharpFunctionalExtensions;
using Shared;
using Shared.Abstractions;
using Shared.Dto;
using Shared.ValueObjects;
using Users.Application.Commands.CreateBasketCommand;
using Users.Application.Commands.UpdateBasketItemCommand;
using Users.Application.Commands.UpdateUserCommand;
using Users.Application.Queries.GetBasketByIdQuery;
using Users.Application.Queries.GetBasketItemByIdQuery;
using Users.Application.Queries.GetUserByIdQuery;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Controllers;

public class UsersContracts(
    ICommandHandler<BasketDto, CreateBasketCommand> createBasketHandler,
    IQueryHandler<GetUserResponseDto?, GetUserByIdQuery> getUserByIdHandler,
    IQueryHandler<BasketItemDto?, GetBasketItemByIdQuery> getBasketItemByIdHandler,
    IQueryHandler<BasketDto?, GetBasketByIdQuery> getBasketByIdHandler,
    UpdateUserHandler updateUserHandler,
    UpdateBasketItemHandler updateBasketItemHandler): IUsersContracts
{
    public async Task<Result<BasketDto, Error>> CreateBasket(
        CreateBasketRequestDto dto,
        CancellationToken cancellationToken)
    {
        var basket = await createBasketHandler.Handle(new CreateBasketCommand(dto), cancellationToken);
        return basket;
    }

    public async Task<GetUserResponseDto?> GetUserById(
        UserWithOnlyIdDto dto,
        CancellationToken cancellationToken)
    {
        var user = await getUserByIdHandler.Handle(new GetUserByIdQuery(dto), cancellationToken);
        return user;
    }

    public async Task<BasketDto?> GetBasketById(
        BasketId basketId,
        CancellationToken cancellationToken)
    {
        var user = await getBasketByIdHandler.Handle(new GetBasketByIdQuery(basketId), cancellationToken);
        return user;
    }

    public async Task<BasketItemDto?> GetBasketItemById(
        BasketItemId basketItemId,
        CancellationToken cancellationToken)
    {
        var basketItem = await getBasketItemByIdHandler.Handle(new GetBasketItemByIdQuery(basketItemId), cancellationToken);
        return basketItem;
    }

    public async Task<Result<UserId, Error>> UpdateUser(
        UserId userId,
        UpdateEntityRequestDto dto,
        CancellationToken cancellationToken)
    {
        var user = await updateUserHandler.Handle(userId.Value, dto, cancellationToken);
        return user;
    }

    public async Task<Result<BasketItemId, Error>> UpdateBasketItem(
        BasketItemId basketItemId,
        UpdateEntityRequestDto dto,
        CancellationToken cancellationToken)
    {
        var basket = await updateBasketItemHandler.Handle(basketItemId.Value, dto, cancellationToken);
        return basket;
    }
}