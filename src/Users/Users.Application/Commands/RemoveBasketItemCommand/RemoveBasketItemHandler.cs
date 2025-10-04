using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;
using Users.Contracts.Dtos;

namespace Users.Application.Commands.RemoveBasketItemCommand;

public class RemoveBasketItemHandler(
    IBasketsRepository basketsRepository,
    ILogger<RemoveBasketItemHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand>
{
    public async Task<Result<RemoveBasketItemDto?, Error>> Handle(
        RemoveBasketItemCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while removing product from basket");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;




        var basketId = new BasketId(command.BasketItemDto.BusketId);
        var basket = await basketsRepository.GetById(basketId, cancellationToken);

        if (basket is null)
        {
            logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return Error.NotFound("remove.basket_item", $"Basket {basketId.Value} not found");
        }

        var basketItem = await basketsRepository.GetBasketItemById(
            new BasketItemId(command.BasketItemDto.BasketItemId),
            cancellationToken);

        if (basketItem is null)
        {
            logger.LogError("Basket item {basketItemId} not found", basketItem?.Id.Value);
            transactionScope.Rollback();
            return Error.NotFound(
                "remove.basket_item",
                $"Basket item {basketItem?.Id.Value} not found");
        }

        var addResult = basket.RemoveItem(basketItem);

        if (addResult.IsFailure)
        {
            logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return addResult.Error;
        }


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while removing product from basket");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Basket item {basketItemId} successfully added", basketItem.Id.Value);

        return command.BasketItemDto;
    }
}