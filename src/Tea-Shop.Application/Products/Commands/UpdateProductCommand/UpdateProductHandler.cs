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
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<CreateProductHandler> _logger;

    public UpdateProductHandler(
        IProductsRepository productsRepository,
        IReadDbContext readDbContext,
        ILogger<CreateProductHandler> logger)
    {
        _productsRepository = productsRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        Product? product = await _productsRepository.GetProductById(
            productId,
            cancellationToken);

        if (product is null)
        {
            return Error.NotFound("update product", "product not found");
        }

        try
        {
            productUpdates.ApplyTo(product);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "update product error");
            return Error.Validation("update.product", "update product error");
        }

        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }
}