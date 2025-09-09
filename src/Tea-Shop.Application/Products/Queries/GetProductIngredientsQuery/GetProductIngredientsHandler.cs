using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application.Products.Queries.GetProductIngredientsQuery;

public class GetProductIngredientsHandler: IQueryHandler<
    GetIngrendientsResponseDto[],
    GetProductsIngredientsQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetProductIngredientsHandler> _logger;

    public GetProductIngredientsHandler(
        IReadDbContext readDbContext,
        ILogger<GetProductIngredientsHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<GetIngrendientsResponseDto[]> Handle(
        GetProductsIngredientsQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetProductIngredientsHandler));

        var product = await _readDbContext.ProductsRead
            .FirstOrDefaultAsync(
                p => p.Id == new ProductId(query.Request.ProductId),
                cancellationToken);

        if (product is null)
        {
            _logger.LogWarning(
                "Product with id {productId} not found",
                query.Request.ProductId);
            return [];
        }

        var ingredients = product.PreparationMethod.Ingredients;

        var ingredientsResponse = ingredients
            .Select(i => new GetIngrendientsResponseDto(
                i.Name,
                i.Amount,
                i.Description,
                i.IsAllergen))
            .ToArray();

        _logger.LogDebug(
            "Get product with id {productId}.",
            query.Request.ProductId);

        return ingredientsResponse;
    }
}