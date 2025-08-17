using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application;

public interface IProductsRepository
{
    Task<Guid> GetProduct(Guid productId, CancellationToken cancellationToken);

    Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken);

    Task<Guid> UpdateProductPrice(Guid productId, float price, CancellationToken cancellationToken);

    Task<Guid> DeleteProduct(Guid productId, CancellationToken cancellationToken);
}