using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Infrastructure.Postgres;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Queries;

public class GetProductIngredientsHandler
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
        GetProductIngridientsRequestDto query,
        CancellationToken cancellationToken)
    {
        var product = await _readDbContext.ProductsRead
            .FirstOrDefaultAsync(p => p.Id == new ProductId(query.ProductId), cancellationToken);

        if (product is null)
        {
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

        return ingredientsResponse;
    }
}