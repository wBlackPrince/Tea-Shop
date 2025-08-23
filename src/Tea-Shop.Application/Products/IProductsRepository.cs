using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application.Products;

public interface IProductsRepository
{
    Task<GetProductResponseDto> GetProduct(ProductId productId, CancellationToken cancellationToken);

    Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken);

    Task<Guid> UpdateProductPrice(Guid productId, float price, CancellationToken cancellationToken);

    Task<Guid> DeleteProduct(ProductId productId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken);
}