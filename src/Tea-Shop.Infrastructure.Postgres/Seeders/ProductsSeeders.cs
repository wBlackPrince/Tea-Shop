namespace Tea_Shop.Infrastructure.Postgres.Seeders;

public class ProductsSeeders: ISeeder
{
    private readonly ProductsDbContext _dbContext;

    public ProductsSeeders(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SeedAsync()
    {
        throw new NotImplementedException();
    }
}