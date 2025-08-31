using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Infrastructure.Postgres;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Queries;

public class GetProductByIdHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(
        IReadDbContext readDbContext,
        ILogger<GetProductByIdHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<GetProductByIdResponseDto?> Handle(
        GetProductByIdRequestDto query,
        CancellationToken cancellationToken)
    {
        Product? product = await _readDbContext.ProductsRead
            .Include(p => p.TagsIds)
            .FirstOrDefaultAsync(
                p => p.Id == new ProductId(query.ProductId),
                cancellationToken);

        if (product is null)
        {
            return null;
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

        var productGetDto = new GetProductByIdResponseDto(
            product.Id.Value,
            product.Title,
            product.Price,
            product.Amount,
            product.StockQuantity,
            product.Description,
            product.Season.ToString(),
            ingrindientsGetDto,
            product.PreparationMethod.Description,
            product.PreparationMethod.PreparationTime,
            product.CreatedAt,
            product.UpdatedAt,
            tagsIds,
            product.PhotosIds);

        _logger.LogInformation("Get product {productId}", query.ProductId);

        return productGetDto;
    }
}