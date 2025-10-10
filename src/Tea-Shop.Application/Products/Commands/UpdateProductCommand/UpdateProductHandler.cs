using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UpdateProductCommand;

public class UpdateProductHandler(
    IProductsRepository productsRepository,
    ILogger<CreateProductHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<Guid, UpdateProductCommand>
{
    public async Task<Result<Guid, Error>> Handle(
        UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while updating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        Product? product = await productsRepository.GetProductById(
            command.ProductId,
            cancellationToken);

        if (product is null)
        {
            transactionScope.Rollback();
            return Error.NotFound("update product", "product not found");
        }

        UnitResult<Error> result = new UnitResult<Error>();

        switch (command.ProductUpdates.Property)
        {
            case nameof(product.Title):
                result = product.UpdateTitle(command.ProductUpdates.NewValue);
                break;
            case nameof(product.Amount):
                result = product.UpdateAmount(float.Parse(command.ProductUpdates.NewValue));
                break;
            case nameof(product.Description):
                result = product.UpdateDescription(command.ProductUpdates.NewValue);
                break;
            case nameof(product.Price):
                result = product.UpdatePrice(float.Parse(command.ProductUpdates.NewValue));
                break;
            case nameof(product.StockQuantity):
                result = product.UpdateStockQuantity(int.Parse(command.ProductUpdates.NewValue));
                break;
            case nameof(product.Season):
                product.Season = (Season)Enum.Parse(typeof(Season), command.ProductUpdates.NewValue);
                break;
            default:
                return Error.NotFound("update.product", "invalid property to update");
        }

        if (result.IsFailure)
        {
            logger.LogError("fail to update product");
            transactionScope.Rollback();
            return result.Error;
        }

        product.UpdatedAt = DateTime.UtcNow.ToUniversalTime();



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }



        logger.LogInformation("Update product {productId}", command.ProductId);

        return product.Id.Value;
    }
}