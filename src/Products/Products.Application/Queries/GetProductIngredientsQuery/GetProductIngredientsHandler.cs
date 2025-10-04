using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Contracts.Dtos;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Products.Application.Queries.GetProductIngredientsQuery;

public class GetProductIngredientsHandler(
    IProductsReadDbContext readDbContext,
    ILogger<GetProductIngredientsHandler> logger):
    IQueryHandler<GetIngrendientsResponseDto[], GetProductsIngredientsQuery>
{
    public async Task<GetIngrendientsResponseDto[]> Handle(
        GetProductsIngredientsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetProductIngredientsHandler));

        var product = await readDbContext.ProductsRead
            .FirstOrDefaultAsync(
                p => p.Id == new ProductId(query.Request.ProductId),
                cancellationToken);

        if (product is null)
        {
            logger.LogWarning(
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

        logger.LogDebug(
            "Get product with id {productId}.",
            query.Request.ProductId);

        return ingredientsResponse;
    }
}