using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ProductsEfCoreRepository: IProductsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetProductById(
        ProductId productId,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Include(p => p.TagsIds)
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        return product;
    }


    public async Task<Ingrendient[]> GetProductIngredients(
        ProductId productId,
        CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products.FirstOrDefaultAsync(
            p => p.Id == productId,
            cancellationToken);

        if (products is null)
        {
            return [];
        }

        return products.PreparationMethod.Ingredients.ToArray();
    }

    public async Task<Product[]> GetProductsByTag(
        TagId tagId,
        CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products
            .Where(p => p.TagsIds.Any(t => t.TagId == tagId))
            .Include(p => p.TagsIds)
            .ToArrayAsync(cancellationToken);

        return products;
    }

    public async Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);

        return product.Id.Value;
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