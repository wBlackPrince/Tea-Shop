using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UpdateProductCommand;

public class UpdateProductHandler
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public UpdateProductHandler(
        IProductsRepository productsRepository,
        ILogger<CreateProductHandler> logger,
        ITransactionManager transactionManager)
    {
        _productsRepository = productsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while updating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        Product? product = await _productsRepository.GetProductById(
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
            _logger.LogError(e, e.Message);
            transactionScope.Rollback();
            return Error.Validation("update.product", e.Message);
        }

        product.UpdatedAt = DateTime.UtcNow.ToUniversalTime();



        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while updating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }



        _logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }
}