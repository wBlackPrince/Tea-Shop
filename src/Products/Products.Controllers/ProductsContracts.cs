using CSharpFunctionalExtensions;
using Products.Application.Commands.CreateProductCommand;
using Products.Application.Commands.UpdateProductCommand;
using Products.Application.Queries.GetProductByIdQuery;
using Products.Contracts;
using Products.Contracts.Dtos;
using Shared;
using Shared.Abstractions;
using Shared.Dto;

namespace Products.Controllers;

public class ProductsContracts(
    IQueryHandler<GetProductDto?, GetProductByIdQuery> getProductsByIdHandler,
    UpdateProductHandler updateProductHandler): IProductsContracts
{
    public async Task<GetProductDto?> GetProductById(ProductWithOnlyIdDto dto, CancellationToken cancellationToken)
    {
        return await getProductsByIdHandler.Handle(new GetProductByIdQuery(dto), cancellationToken);
    }

    public async Task<Result<Guid, Error>> UpdateProduct(
        Guid productId,
        UpdateEntityRequestDto dto,
        CancellationToken cancellationToken)
    {
        return await updateProductHandler.Handle(productId, dto, cancellationToken);
    }
}