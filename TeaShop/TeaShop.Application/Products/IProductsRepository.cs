using TeaShopDomain.Products;

namespace TeaShop.Application.Products;

public interface IProductsRepository
{
    Task<Guid> AddAsync(Product product, CancellationToken cancellationToken);

    Task<Guid> SaveAsync(Product product, CancellationToken cancellationToken);

    Task<Guid> DeleteAsync(Guid productId, CancellationToken cancellationToken);

    Task<Product> GetAsyncById(Guid productId, CancellationToken cancellationToken);

    Task<int> GetProductCountWithSimilarName(string productName, CancellationToken cancellationToken);
}