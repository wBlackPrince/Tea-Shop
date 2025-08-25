using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ProductsEfCoreRepository: IProductsRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Product, Error>> GetProductById(
        ProductId productId,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Include(p => p.TagsIds)
            .FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

        if (product is null)
        {
            return Error.NotFound("product.get", "Product not found");
        }

        return product;
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