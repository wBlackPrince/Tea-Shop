using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products.Commands.UpdateProductCommand;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.DeleteProductCommand;

public class DeleteProductHandler: ICommandHandler<
    DeleteProductDto,
    DeleteProductQuery>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<UpdateProductHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public DeleteProductHandler(
        IProductsRepository productsRepository,
        ILogger<UpdateProductHandler> logger,
        ITransactionManager transactionManager)
    {
        _productsRepository = productsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<DeleteProductDto, Error>> Handle(
        DeleteProductQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteProductHandler));


        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while deleting product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        var deleteResult = await _productsRepository.DeleteProduct(
            new ProductId(query.Request.ProductId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogError(
                "Failed to delete product {productId}",
                query.Request.ProductId);
            transactionScope.Rollback();

            return deleteResult.Error;
        }



        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while deleting product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogError("Delete product {productId}", query.Request.ProductId);

        return query.Request;
    }
}