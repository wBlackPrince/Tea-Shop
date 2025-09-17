using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
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

    public DeleteProductHandler(
        IProductsRepository productsRepository,
        ILogger<UpdateProductHandler> logger)
    {
        _productsRepository = productsRepository;
        _logger = logger;
    }

    public async Task<Result<DeleteProductDto, Error>> Handle(
        DeleteProductQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteProductHandler));

        var deleteResult = await _productsRepository.DeleteProduct(
            new ProductId(query.Request.ProductId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogError(
                "Failed to delete product {productId}",
                query.Request.ProductId);

            return deleteResult.Error;
        }

        await _productsRepository.SaveChangesAsync(cancellationToken);
        _logger.LogError("Delete product {productId}", query.Request.ProductId);

        return query.Request;
    }
}