using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ProductsRepository: IProductsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductsRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(
            p => p.Id == new ProductId(productId),
            cancellationToken);

        return product;
    }

    public async Task<Result<Guid, Error>> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Products.AddAsync(product, cancellationToken);

            return product.Id.Value;
        }
        catch (Exception e)
        {
            return Error.Failure("create.product", "failed to create product");
        }
    }

    public async Task<Result<Guid, Error>> DeleteProduct(
        ProductId productId,
        CancellationToken cancellationToken)
    {
        var foundResult = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        if (foundResult is null)
        {
            return Error.NotFound("product.get", "Product not found");
        }

        await _dbContext.Products
            .Where(p => p.Id == productId)
            .ExecuteDeleteAsync(cancellationToken);

        return productId.Value;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}