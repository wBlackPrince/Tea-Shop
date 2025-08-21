using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products;

public interface IProductsService
{
    Task<Guid> GetProduct(
        Guid productId,
        CancellationToken cancellationToken);

    Task<Guid> CreateProduct(
        CreateProductRequestDto request,
        CancellationToken cancellationToken);

    Task<Guid> UpdateProductPrice(
        Guid productId,
        UpdateProductPriceRequestDto request,
        CancellationToken cancellationToken);

    Task<Guid> DeleteProduct(
        Guid productId,
        CancellationToken cancellationToken);

    Task<Guid> CreateOrder(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken);
}