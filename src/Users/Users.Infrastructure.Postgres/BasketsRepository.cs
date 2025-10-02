using System.Diagnostics;
using Baskets.Application;
using Baskets.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.ValueObjects;

namespace Baskets.Infrastructure.Postgres;

public class BasketsRepository : IBasketsRepository
{
    private readonly BasketsDbContext _dbContext;

    public BasketsRepository(BasketsDbContext dbContext)
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
        await _dbContext.Buskets.AddAsync(basket, cancellationToken);

        return basket.Id.Value;
    }
}