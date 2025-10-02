using Microsoft.EntityFrameworkCore;
using Products.Application;
using Products.Domain;

namespace Products.Infrastructure.Postgres;

public class ProductsDbContext: DbContext, IProductsReadDbContext
{
    private readonly string _connectionString;

    public ProductsDbContext()
    {
    }

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

    public DbSet<Tag> Tags { get; set; }

    public DbSet<ProductsTags> ProductsTags { get; set; }

    public IQueryable<Product> ProductsRead => Set<Product>().AsNoTracking();

    public IQueryable<Tag> TagsRead => Set<Tag>().AsNoTracking();
}