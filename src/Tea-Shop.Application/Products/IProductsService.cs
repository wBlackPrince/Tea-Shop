using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products;

public interface IProductsService
{
    Task<Result<GetProductResponseDto, Error>> GetProductById(
        Guid productId,
        CancellationToken cancellationToken);

    Task<Result<GetProductResponseDto[], Error>> GetProductsByTag(
        Guid tagId,
        CancellationToken cancellationToken);

    Task<Result<GetIngrendientsResponseDto[], Error>> GetProductIngredients(
        Guid productId,
        CancellationToken cancellationToken);

    Task<CreateProductResponseDto> CreateProduct(
        CreateProductRequestDto request,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> UpdateProduct(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteProduct(
        Guid productId,
        CancellationToken cancellationToken);
}