using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Buskets;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres;

public class ProductsDbContext: DbContext, IReadDbContext
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

    public DbSet<User> Users { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Busket> Buskets { get; set; }

    public DbSet<BusketItem> BusketsItems { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<ProductsTags> ProductsTags { get; set; }


    public IQueryable<Product> ProductsRead => Set<Product>().AsNoTracking();

    public IQueryable<Order> OrdersRead => Set<Order>().AsNoTracking();

    public IQueryable<Comment> CommentsRead => Set<Comment>().AsNoTracking();

    public IQueryable<Review> ReviewsRead => Set<Review>().AsNoTracking();

    public IQueryable<User> UsersRead => Set<User>().AsNoTracking();

    public IQueryable<Busket> BusketsRead => Set<Busket>().AsNoTracking();

    public IQueryable<BusketItem> BusketsItemsRead => Set<BusketItem>().AsNoTracking();
}