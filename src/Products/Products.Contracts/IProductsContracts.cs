using CSharpFunctionalExtensions;
using Products.Contracts.Dtos;
using Shared;
using Shared.Dto;

namespace Products.Contracts;

public interface IProductsContracts
{
    Task<GetProductDto?> GetProductById(ProductWithOnlyIdDto dto, CancellationToken cancellationToken);
    
    Task<Result<Guid, Error>> UpdateProduct(
        Guid ProductId,
        UpdateEntityRequestDto dto,
        CancellationToken cancellationToken);
}