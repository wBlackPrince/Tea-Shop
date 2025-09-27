using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Baskets;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Baskets.Commands.AddBasketItemCommand;

public class AddBasketItemHandler(
    IBasketsRepository basketsRepository,
    ILogger<AddBasketItemHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<AddBasketItemDto?,  AddBasketItemCommand>
{
    public async Task<Result<AddBasketItemDto?, Error>> Handle(
        AddBasketItemCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while adding product to basket");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;




        var basketId = new BasketId(command.AddBasketItemDto.BusketId);
        var basket = await basketsRepository.GetById(basketId, cancellationToken);

        if (basket is null)
        {
            logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return Error.NotFound("add.basket_item", $"Basket {basketId.Value} not found");
        }

        var basketItem = new BasketItem(
            new BasketItemId(Guid.NewGuid()),
            new BasketId(command.AddBasketItemDto.BusketId),
            new ProductId(command.AddBasketItemDto.ProductId),
            command.AddBasketItemDto.Quantity);

        var addResult = basket.AddItem(basketItem);

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
            logger.LogError("Failed to commit result while adding product to basket");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Basket item {basketItemId} successfully added", basketItem.Id.Value);

        return command.AddBasketItemDto;
    }
}