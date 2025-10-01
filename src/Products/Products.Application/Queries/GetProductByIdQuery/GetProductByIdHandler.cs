namespace Products.Application.Queries.GetProductByIdQuery;

public class GetProductByIdHandler(IReadDbContext readDbContext, ILogger<GetProductByIdHandler> logger):
    IQueryHandler<GetProductDto?, GetProductByIdQuery>
{
    public async Task<GetProductDto?> Handle(
        GetProductByIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetProductByIdHandler));

        Product? product = await readDbContext.ProductsRead
            .Include(p => p.TagsIds)
            .FirstOrDefaultAsync(
                p => p.Id == new ProductId(query.Request.ProductId),
                cancellationToken);

        if (product is null)
        {
            logger.LogWarning("Product with id {productId} not found", query.Request.ProductId);
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

        logger.LogDebug("Get product {productId}", query.Request.ProductId);

        return productGetDto;
    }
}