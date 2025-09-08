using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;

namespace Tea_Shop.Application.Products.Queries.GetProductsQuery;

public class GetProductsHandler: IQueryHandler<GetProductsResponseDto, GetProductsQuery>
{
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<GetProductsHandler> _logger;

    public GetProductsHandler(
        IReadDbContext readDbContext,
        ILogger<GetProductsHandler> logger)
    {
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<GetProductsResponseDto> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var productsQuery = _readDbContext.ProductsRead;

        if (!string.IsNullOrEmpty(query.Request.Search))
            productsQuery = productsQuery.Where(p => EF.Functions.Like(
                p.Title.ToLower(), 
                $"%{query.Request.Search.ToLower()}%"));

        if (query.Request.TagId is not null)
            productsQuery = productsQuery.Where(p => p.TagsIds.Any(ti => ti.TagId == new TagId(query.Request.TagId.Value)));

        if (!string.IsNullOrEmpty(query.Request.Season))
        {
            Season enumSeason = (Season)Enum.Parse(typeof(Season), query.Request.Season);

            productsQuery = productsQuery.Where(p => p.Season == enumSeason);
        }

        if (query.Request.MinPrice is not null)
        {
            productsQuery = productsQuery.Where(p =>
                p.Price >= query.Request.MinPrice &&
                p.Price <= query.Request.MaxPrice);
        }

        long totalCount = await productsQuery.LongCountAsync(cancellationToken);

        productsQuery = productsQuery
            .OrderBy(p => p.CreatedAt)
            .Skip((query.Request.Pagination.Page - 1) * query.Request.Pagination.PageSize)
            .Take(query.Request.Pagination.PageSize);

        var products = await productsQuery
            .Select(p => new ProductDto()
            {
                Id = p.Id.Value,
                Title = p.Title,
                Price = p.Price,
                Amount = p.Amount,
                StockQuantity = p.StockQuantity,
                Description = p.Description,
                Season = p.Season.ToString(),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                TagsIds = p.TagsIds.Select(ti => ti.TagId.Value).ToArray(),
                PhotosIds = p.PhotosIds,
            })
            .ToArrayAsync();

        _logger.LogInformation($"Get products by tag: {query.Request.TagId}");

        return new GetProductsResponseDto(products, totalCount);
    }
}