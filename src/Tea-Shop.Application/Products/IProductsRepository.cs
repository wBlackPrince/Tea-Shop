using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products;

public interface IProductsRepository
{
    Task<Product?> GetProductById(Guid productId, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> CreateProduct(Product product, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteProduct(ProductId productId, CancellationToken cancellationToken);

    Task<Guid> CreateTag(Tag tag, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteTag(TagId tagId, CancellationToken cancellationToken);
}