using Tea_Shop.Application.Buskets;
using Tea_Shop.Domain.Buskets;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class BusketsRepository : IBusketsRepository
{
    private readonly ProductsDbContext _dbContext;

    public BusketsRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Create(Busket busket, CancellationToken cancellationToken)
    {
        await _dbContext.Buskets.AddAsync(busket, cancellationToken);

        return busket.Id.Value;
    }
}