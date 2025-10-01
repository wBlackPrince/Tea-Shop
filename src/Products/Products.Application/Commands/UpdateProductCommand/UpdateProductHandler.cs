using System.Data;
using Products.Application.Commands.CreateProductCommand;

namespace Products.Application.Commands.UpdateProductCommand;

public class UpdateProductHandler(
    IProductsRepository productsRepository,
    ILogger<CreateProductHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
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
            productId,
            cancellationToken);

        if (product is null)
        {
            transactionScope.Rollback();
            return Error.NotFound("update product", "product not found");
        }

        try
        {
            productUpdates.ApplyTo(product);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            transactionScope.Rollback();
            return Error.Validation("update.product", e.Message);
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



        logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }
}