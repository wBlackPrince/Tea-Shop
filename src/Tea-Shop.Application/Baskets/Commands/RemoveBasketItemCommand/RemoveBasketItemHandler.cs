using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Baskets;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Baskets.Commands.RemoveBasketItemCommand;

public class RemoveBasketItemHandler: ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand>
{
    private readonly IBasketsRepository _basketsRepository;
    private readonly ILogger<RemoveBasketItemHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public RemoveBasketItemHandler(
        IBasketsRepository basketsRepository,
        ILogger<RemoveBasketItemHandler> logger,
        ITransactionManager transactionManager)
    {
        _basketsRepository = basketsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<RemoveBasketItemDto?, Error>> Handle(
        RemoveBasketItemCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while removing product from basket");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;




        var basketId = new BasketId(command.BasketItemDto.BusketId);
        var basket = await _basketsRepository.GetById(basketId, cancellationToken);

        if (basket is null)
        {
            _logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return Error.NotFound("remove.basket_item", $"Basket {basketId.Value} not found");
        }

        var basketItem = await _basketsRepository.GetBasketItemById(
            new BasketItemId(command.BasketItemDto.BasketItemId),
            cancellationToken);

        if (basketItem is null)
        {
            _logger.LogError("Basket item {basketItemId} not found", basketItem?.Id.Value);
            transactionScope.Rollback();
            return Error.NotFound(
                "remove.basket_item",
                $"Basket item {basketItem?.Id.Value} not found");
        }

        var addResult = basket.RemoveItem(basketItem);

        if (addResult.IsFailure)
        {
            _logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return addResult.Error;
        }


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while removing product from basket");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Basket item {basketItemId} successfully added", basketItem.Id.Value);

        return command.BasketItemDto;
    }
}