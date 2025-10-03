using System.ComponentModel.DataAnnotations;
using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;
using Shared.Dto;
using Shared.ValueObjects;
using Users.Domain;

namespace Users.Application.Commands.UpdateBasketItemCommand;

public class UpdateBasketItemHandler(
    IUsersRepository usersRepository,
    ILogger<UpdateBasketItemHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<BasketItemId, Error>> Handle(
        Guid basketItemId,
        UpdateEntityRequestDto request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handleName}", nameof(UpdateBasketItemHandler));


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while updating basket");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var basketItem = await usersRepository.GetBasketItemById(
            new BasketItemId(basketItemId),
            cancellationToken);

        if (basketItem is null)
        {
            logger.LogWarning("Basket item not found while updating");
            transactionScope.Rollback();
            return Error.NotFound("update.basket_item", "basket item not found");
        }

        try
        {
            switch (request.Property)
            {
                case nameof(basketItem.Quantity):
                    basketItem.Quantity = (int)request.NewValue;
                    break;
                default:
                    throw new ValidationException("Invalid property");
            }
        }
        catch (Exception e)
        {
            logger.LogError("Validation error while updating product");
            transactionScope.Rollback();
            return Error.Validation("update product", e.Message);
        }



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogDebug("Update basket item with id {BasketItemId}", basketItemId);

        return basketItem.Id;
    }
}