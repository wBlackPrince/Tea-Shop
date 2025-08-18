using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres;

public class ProductsEfCoreRepository: IProductsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> GetProduct(Guid productId, CancellationToken cancellationToken)
    {
        var result = _dbContext.Products
            .FirstOrDefault(p => p.Id == productId);

        return result.Id;
    }

    public async Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);

        return product.Id;
    }

    public async Task<Guid> UpdateProductPrice(
        Guid productId,
        float price,
        CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(p => p.Id == productId)
            .ExecuteUpdateAsync(setter
                => setter.SetProperty(p => p.Price, price));

        return productId;
    }

    public async Task<Guid> DeleteProduct(
        Guid productId,
        CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(p => p.Id == productId)
            .ExecuteDeleteAsync();

        return productId;
    }
}