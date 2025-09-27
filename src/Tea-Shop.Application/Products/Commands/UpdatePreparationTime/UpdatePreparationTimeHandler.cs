using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UpdatePreparationTime;

public class UpdatePreparationTimeHandler(
    IProductsRepository productsRepository,
    ILogger<UpdatePreparationTimeHandler> logger,
    ITransactionManager transactionManager):
    ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationTimeCommand>
{
    public async Task<Result<ProductWithOnlyIdDto, Error>> Handle(
        UpdatePreparationTimeCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handlerName}", nameof(UpdatePreparationTimeHandler));
        Product? product = await productsRepository.GetProductById(
            command.Request.ProductId,
            cancellationToken);


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        if (product is null)
        {
            logger.LogError("Product with id {productId} not found", command.Request.ProductId);
            transactionScope.Rollback();
            return Error.NotFound("update.preparation.method", "Product not found");
        }


        var updateResult = product.PreparationMethod.UpdatePreparationTime(command.Request.PreparationTime);

        if (updateResult.IsFailure)
        {
            logger.LogError("Failed to update preparation time");
            transactionScope.Rollback();
            return updateResult.Error;
        }

        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Updated preparation time");

        var response = new ProductWithOnlyIdDto(command.Request.ProductId);

        return response;
    }
}