using System.Diagnostics;

namespace Baskets.Infrastructure.Postgres;

public class BasketsRepository : IBasketsRepository
{
    private readonly ProductsDbContext _dbContext;

    public BasketsRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Basket?> GetById(BasketId basketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Buskets.FirstOrDefaultAsync(b => b.Id == basketId, cancellationToken);
    }

    public async Task<BasketItem?> GetBasketItemById(BasketItemId basketItemId, CancellationToken cancellationToken)
    {
        return await _dbContext.BusketsItems.FirstOrDefaultAsync(b => b.Id == basketItemId, cancellationToken);
    }

    public async Task<Guid> Create(Basket basket, CancellationToken cancellationToken)
    {
        var entry = await _dbContext.Buskets.AddAsync(basket, cancellationToken);

        Debug.Assert(entry.State == EntityState.Added);

        return basket.Id.Value;
    }
}