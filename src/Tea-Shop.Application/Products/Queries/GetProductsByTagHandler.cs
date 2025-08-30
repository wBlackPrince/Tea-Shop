using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Queries;

public class GetProductsByTagHandler
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<GetProductsByTagHandler> _logger;

    public GetProductsByTagHandler(
        IProductsRepository productsRepository,
        ILogger<GetProductsByTagHandler> logger)
    {
        _productsRepository = productsRepository;
        _logger = logger;
    }

    public async Task<Result<GetProductResponseDto[], Error>> Handle(
        Guid tagId,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, products, error) = await _productsRepository.GetProductsByTag(
            new TagId(tagId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        GetProductResponseDto[] response = new GetProductResponseDto[products.Length];

        for (int i = 0; i < products.Length; i++)
        {
            response[i] = new GetProductResponseDto(
                products[i].Id.Value,
                products[i].Title,
                products[i].Price,
                products[i].Amount,
                products[i].Description,
                products[i].Season.ToString(),
                products[i].PreparationMethod.Ingredients
                    .Select(ing =>
                        new GetIngrendientsResponseDto(ing.Name, ing.Amount, ing.Description, ing.IsAllergen))
                    .ToArray(),
                products[i].PreparationMethod.Description,
                products[i].PreparationMethod.PreparationTime,
                products[i].CreatedAt,
                products[i].UpdatedAt,
                products[i].TagsIds.Select(ti => ti.TagId.Value).ToArray(),
                products[i].PhotosIds);
        }

        return response;
    }
}