using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products;

public interface IProductsRepository
{
    Task<Result<Product, Error>> GetProductById(
        ProductId productId,
        CancellationToken cancellationToken);

    Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken);

    Task<Guid> DeleteProduct(ProductId productId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken);
}