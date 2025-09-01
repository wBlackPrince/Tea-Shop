using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands;

public class DeleteProductHandler
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

    public async Task<Result<Guid, Error>> Handle(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _productsRepository.DeleteProduct(
            new ProductId(productId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogInformation("Failed to delete product {productId}", productId);

            return deleteResult.Error;
        }

        await _productsRepository.SaveChangesAsync(cancellationToken);
        _logger.LogError("Delete product {productId}", productId);

        return productId;
    }
}