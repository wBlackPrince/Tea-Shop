using Microsoft.EntityFrameworkCore;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres;

public class ProductsDbContext: DbContext
{
    private readonly string _connectionString;

    public ProductsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
    }

    public DbSet<Product> Products { get; set; }
}