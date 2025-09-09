using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application.Products.Queries.GetProductByIdQuery;

public class GetProductByIdHandler: IQueryHandler<GetProductDto?, GetProductByIdQuery>
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

    public async Task<GetProductDto?> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetProductByIdHandler));

        Product? product = await _readDbContext.ProductsRead
            .Include(p => p.TagsIds)
            .FirstOrDefaultAsync(
                p => p.Id == new ProductId(query.Request.ProductId),
                cancellationToken);

        if (product is null)
        {
            _logger.LogWarning("Product with id {productId} not found", query.Request.ProductId);
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

        var productGetDto = new GetProductDto(
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

        _logger.LogDebug("Get product {productId}", query.Request.ProductId);

        return productGetDto;
    }
}