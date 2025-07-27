using TeaShop.Application.Products;
using TeaShopDomain.Products;

namespace TeaShop.Infrastructure.Postgres;

public class ProductsEfCoreRepository: IProductsRepository
{
    private readonly ProductsDbContext _dbContext;

    public async Task<Guid> AddAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    public Task<Guid> SaveAsync(Product product, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> DeleteAsync(Guid productId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetAsyncById(Guid productId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetProductCountWithSimilarName(string productName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}