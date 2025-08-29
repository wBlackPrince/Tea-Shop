using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;


public class OrdersRepository
{
    private readonly ProductsDbContext _dbContext;

    public OrdersRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
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