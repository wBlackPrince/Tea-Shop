using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Queries;

public class GetProductByIdHandler
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(
        IProductsRepository productsRepository,
        ILogger<GetProductByIdHandler> logger)
    {
        _productsRepository = productsRepository;
        _logger = logger;
    }

    public async Task<Result<GetProductResponseDto, Error>> Handler(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, product, error) = await _productsRepository.GetProductById(
            new ProductId(productId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        var ingrindientsGetDto = product.PreparationMethod.Ingredients
            .Select(i => new GetIngrendientsResponseDto(
                i.Name,
                i.Amount,
                i.Description,
                i.IsAllergen)).ToArray();

        var tagsIds = product.TagsIds
            .Select(i => i.Id.Value)
            .ToArray();

        var productGetDto = new GetProductResponseDto(
            product.Id.Value,
            product.Title,
            product.Price,
            product.Amount,
            product.Description,
            product.Season.ToString(),
            ingrindientsGetDto,
            product.PreparationMethod.Description,
            product.PreparationMethod.PreparationTime,
            product.CreatedAt,
            product.UpdatedAt,
            tagsIds,
            product.PhotosIds);

        _logger.LogInformation("Get product {productId}", productId);

        return productGetDto;
    }
}