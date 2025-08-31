using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Queries;

public class GetProductIngredientsHandler
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<GetProductIngredientsHandler> _logger;

    public GetProductIngredientsHandler(
        IProductsRepository productsRepository,
        ILogger<GetProductIngredientsHandler> logger)
    {
        _productsRepository = productsRepository;
        _logger = logger;
    }

    public async Task<GetIngrendientsResponseDto[]> Handle(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var ingredientsResult = await _productsRepository.GetProductIngredients(
            new ProductId(productId),
            cancellationToken);

        if (ingredientsResult.Length == 0)
        {
            return [];
        }

        var ingredientsResponse = ingredientsResult
            .Select(i => new GetIngrendientsResponseDto(
                i.Name,
                i.Amount,
                i.Description,
                i.IsAllergen))
            .ToArray();

        return ingredientsResponse;
    }
}