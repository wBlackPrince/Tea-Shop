using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands;

public class UpdateProductHandler
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<CreateProductHandler> _logger;

    public UpdateProductHandler(
        IProductsRepository productsRepository,
        ILogger<CreateProductHandler> logger)
    {
        _productsRepository = productsRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handler(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, product, error) = await _productsRepository.GetProductById(
            new ProductId(productId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        productUpdates.ApplyTo(product);
        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }
}