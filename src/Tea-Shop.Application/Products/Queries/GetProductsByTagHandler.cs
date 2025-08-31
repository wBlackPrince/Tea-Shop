using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Infrastructure.Postgres;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Queries;

public class GetProductsByTagHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetProductsByTagHandler> _logger;

    public GetProductsByTagHandler(
        IReadDbContext readDbContext,
        ILogger<GetProductsByTagHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<GetProductByIdResponseDto[]> Handle(
        GetProductsByTagRequestDto query,
        CancellationToken cancellationToken)
    {
        var tagId = new TagId(query.TagId);

        var products = await _readDbContext.ProductsRead
            .Where(p => p.TagsIds.Any(t => t.TagId == tagId))
            .Include(p => p.TagsIds)
            .ToArrayAsync(cancellationToken);

        if (products.Length == 0)
        {
            return [];
        }

        GetProductByIdResponseDto[] response = new GetProductByIdResponseDto[products.Length];

        for (int i = 0; i < products.Length; i++)
        {
            response[i] = new GetProductByIdResponseDto(
                products[i].Id.Value,
                products[i].Title,
                products[i].Price,
                products[i].Amount,
                products[i].StockQuantity,
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