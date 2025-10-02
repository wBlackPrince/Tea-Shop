using Products.Contracts.Dtos;

namespace Products.Contracts;

public interface IProductsContracts
{
    Task<GetProductDto?> GetProductById(ProductWithOnlyIdDto dto, CancellationToken cancellationToken);
}