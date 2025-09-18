using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UpdatePreparationDescription;

public class UpdatePreparationDescriptionHandler:
    ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationDescriptionCommand>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<UpdatePreparationDescriptionCommand> _logger;
    private readonly ITransactionManager _transactionManager;

    public UpdatePreparationDescriptionHandler(
        IProductsRepository productsRepository,
        ILogger<UpdatePreparationDescriptionCommand> logger,
        ITransactionManager transactionManager)
    {
        _productsRepository = productsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<ProductWithOnlyIdDto, Error>> Handle(
        UpdatePreparationDescriptionCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(UpdatePreparationDescriptionHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError(
                "Failed to begin transaction while updating product's preparation description");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var product = await _productsRepository.GetProductById(
            command.Request.ProductId,
            cancellationToken);

        if (product is null)
        {
            _logger.LogError("Product with id {productId} does not exist", command.Request.ProductId);
            transactionScope.Rollback();
            return Error.NotFound(
                "update.product.preparation_description",
                "Product with id {productId} does not exist");
        }

        try
        {
            product.PreparationMethod.Description = command.Request.Description;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating product preparation description");
            transactionScope.Rollback();
            return Error.Failure(
                "update.product.preparation_description",
                "Error while updating product preparation description");
        }


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while updating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug(
            "Updated product's preparation with id {productId}",
            command.Request.ProductId);

        var response = new ProductWithOnlyIdDto(command.Request.ProductId);

        return response;
    }
}