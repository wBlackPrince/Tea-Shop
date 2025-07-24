using TeaShop.Contract.Products;

namespace TeaShop.Application.Products;

public interface IProductService
{
    Task<Guid> Create(CreateProductDto request, CancellationToken cancellationToken);
}