using System.Data;
using Products.Application.Commands.UpdateProductCommand;

namespace Products.Application.Commands.DeleteProductCommand;

public class DeleteProductHandler(
    IProductsRepository productsRepository,
    ILogger<UpdateProductHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<DeleteProductDto, DeleteProductQuery>
{
    public async Task<Result<DeleteProductDto, Error>> Handle(
        DeleteProductQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(DeleteProductHandler));


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while deleting product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var deleteResult = await productsRepository.DeleteProduct(
            new ProductId(query.Request.ProductId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            logger.LogError(
                "Failed to delete product {productId}",
                query.Request.ProductId);
            transactionScope.Rollback();

            return deleteResult.Error;
        }



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while deleting product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogError("Delete product {productId}", query.Request.ProductId);

        return query.Request;
    }
}