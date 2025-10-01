using System.Data;

namespace Products.Application.Commands.UpdatePreparationDescription;

public class UpdatePreparationDescriptionHandler(
    IProductsRepository productsRepository,
    ILogger<UpdatePreparationDescriptionCommand> logger,
    ITransactionManager transactionManager):
    ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationDescriptionCommand>
{
    public async Task<Result<ProductWithOnlyIdDto, Error>> Handle(
        UpdatePreparationDescriptionCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(UpdatePreparationDescriptionHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError(
                "Failed to begin transaction while updating product's preparation description");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var product = await productsRepository.GetProductById(
            command.Request.ProductId,
            cancellationToken);

        if (product is null)
        {
            logger.LogError("Product with id {productId} does not exist", command.Request.ProductId);
            transactionScope.Rollback();
            return Error.NotFound(
                "update.product.preparation_description",
                "Product with id {productId} does not exist");
        }

        var updatedResult = product.PreparationMethod.UpdateDescription(command.Request.Description);

        if (updatedResult.IsFailure)
        {
            logger.LogError("Error while updating product preparation description");
            transactionScope.Rollback();
            return Error.Failure(
                "update.product.preparation_description",
                "Error while updating product preparation description");
        }


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug(
            "Updated product's preparation with id {productId}",
            command.Request.ProductId);

        var response = new ProductWithOnlyIdDto(command.Request.ProductId);

        return response;
    }
}