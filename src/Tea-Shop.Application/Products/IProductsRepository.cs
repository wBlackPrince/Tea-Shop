using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products;

public interface IProductsRepository
{
    Task<Result<Product, Error>> GetProductById(
        ProductId productId,
        CancellationToken cancellationToken);

    Task<Result<Product[], Error>> GetProductsByTag(
        TagId tagId,
        CancellationToken cancellationToken);

    Task<Result<Ingrendient[], Error>> GetProductIngredients(
        ProductId productId,
        CancellationToken cancellationToken);

    Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteProduct(ProductId productId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}