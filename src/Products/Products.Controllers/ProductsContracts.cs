using Products.Application.Commands.CreateProductCommand;
using Products.Application.Queries.GetProductByIdQuery;
using Products.Contracts;
using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Controllers;

public class ProductsContracts(
    IQueryHandler<GetProductDto?, GetProductByIdQuery> _getProductsByIdHandler): IProductsContracts
{
    public async Task<GetProductDto?> GetProductById(ProductWithOnlyIdDto dto, CancellationToken cancellationToken)
    {
        return await _getProductsByIdHandler.Handle(new GetProductByIdQuery(dto), cancellationToken);
    }
}