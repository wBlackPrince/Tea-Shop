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

public class AddBasketItemHandler: ICommandHandler<AddBasketItemDto?,  AddBasketItemCommand>
{
    private readonly IBasketsRepository _basketsRepository;
    private readonly ILogger<AddBasketItemHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public AddBasketItemHandler(
        IBasketsRepository basketsRepository,
        ILogger<AddBasketItemHandler> logger,
        ITransactionManager transactionManager)
    {
        _basketsRepository = basketsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<AddBasketItemDto?, Error>> Handle(
        AddBasketItemCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while adding product to basket");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;




        var basketId = new BasketId(command.AddBasketItemDto.BusketId);
        var basket = await _basketsRepository.GetById(basketId, cancellationToken);

        if (basket is null)
        {
            _logger.LogError("Basket {basketId} not found", basketId.Value);
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
            _logger.LogError("Basket {basketId} not found", basketId.Value);
            transactionScope.Rollback();
            return addResult.Error;
        }


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while adding product to basket");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Basket item {basketItemId} successfully added", basketItem.Id.Value);

        return command.AddBasketItemDto;
    }
}