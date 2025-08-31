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

    public async Task<Result<Guid, Error>> Handle(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        Product? product = await _productsRepository.GetProductById(
            new ProductId(productId),
            cancellationToken);

        if (product is null)
        {
            return Error.NotFound("update product", "product not found");
        }

        productUpdates.ApplyTo(product);
        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }
}