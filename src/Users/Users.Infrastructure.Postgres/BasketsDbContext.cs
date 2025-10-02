using Baskets.Application;
using Baskets.Domain;
using Microsoft.EntityFrameworkCore;

namespace Baskets.Infrastructure.Postgres;

public class BasketsDbContext: DbContext, IBasketsReadDbContext
{
    private readonly string _connectionString;

    public BasketsDbContext()
    {
    }

    public BasketsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketsDbContext).Assembly);
    }

    public DbSet<Basket> Buskets { get; set; }

    public DbSet<BasketItem> BusketsItems { get; set; }

    public IQueryable<Basket> BusketsRead => Set<Basket>().AsNoTracking();

    public IQueryable<BasketItem> BusketsItemsRead => Set<BasketItem>().AsNoTracking();
}