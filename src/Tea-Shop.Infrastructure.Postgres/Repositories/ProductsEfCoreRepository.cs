using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ProductsEfCoreRepository: IProductsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetProductResponseDto> GetProduct(
        ProductId productId,
        CancellationToken cancellationToken)
    {
        var result = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        var ingredientaResponse = result.PreparationMethod.Ingredients
            .Select(i => new GetIngrendientsResponseDto(
                i.Name,
                i.Amount,
                i.Description,
                i.IsAllergen)).ToArray();


        var tagsIds = await _dbContext.ProductsTags
            .Where(pt => pt.Product.Id == productId)
            .Select(pt => pt.TagId.Value)
            .ToArrayAsync();


        var response = new GetProductResponseDto(
            result.Title,
            result.Price,
            result.Amount,
            result.Description,
            result.Season.ToString(),
            ingredientaResponse,
            result.PreparationMethod.Description,
            result.PreparationMethod.PreparationTime,
            tagsIds,
            result.PhotosIds);

        return response;
    }

    public async Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);

        return product.Id.Value;
    }

    public async Task<Guid> UpdateProductPrice(
        Guid productId,
        float price,
        CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(p => p.Id.Value == productId)
            .ExecuteUpdateAsync(setter
                => setter.SetProperty(p => p.Price, price));

        return productId;
    }

    public async Task<Guid> DeleteProduct(
        ProductId productId,
        CancellationToken cancellationToken)
    {
        await _dbContext.Products
            .Where(p => p.Id == productId)
            .ExecuteDeleteAsync(cancellationToken);

        return productId.Value;
    }

    public async Task<Guid> CreateOrder(Order order, CancellationToken cancellationToken)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);

        return order.Id.Value;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}