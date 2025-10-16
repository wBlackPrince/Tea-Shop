using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Commands.AddBasketItemCommand;

public class AddBasketItemHandler(
    IUsersRepository usersRepository,
    IProductsRepository productsRepository,
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
        var basket = await usersRepository.GetBasketById(basketId, cancellationToken);

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

        var product = await productsRepository.GetProductById(
            command.AddBasketItemDto.ProductId,
            cancellationToken);

        if (command.AddBasketItemDto.Quantity > Constants.Limit15)
        {
            logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return Error.Validation("add.basket_item", $"You cannot add more than 15 products to basket");
        }
        else
        {
            product.UpdateStockQuantity(product.StockQuantity - command.AddBasketItemDto.Quantity);
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