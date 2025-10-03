using Microsoft.EntityFrameworkCore;
using Shared.ValueObjects;
using Users.Application;
using Users.Domain;

namespace Users.Infrastructure.Postgres.Repositories;

public class BasketsRepository : IBasketsRepository
{
    private readonly UsersDbContext _dbContext;

    public BasketsRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Basket?> GetById(BasketId basketId, CancellationToken cancellationToken)
    {
        return await _dbContext.Baskets.FirstOrDefaultAsync(b => b.Id == basketId, cancellationToken);
    }

    public async Task<BasketItem?> GetBasketItemById(BasketItemId basketItemId, CancellationToken cancellationToken)
    {
        return await _dbContext.BasketItems.FirstOrDefaultAsync(b => b.Id == basketItemId, cancellationToken);
    }

    public async Task<Guid> Create(Basket basket, CancellationToken cancellationToken)
    {
        await _dbContext.Baskets.AddAsync(basket, cancellationToken);

        return basket.Id.Value;
    }
}