namespace Products.Application;

public interface IProductsRepository
{
    Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> CreateProduct(Product product, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteProduct(ProductId productId, CancellationToken cancellationToken);
}