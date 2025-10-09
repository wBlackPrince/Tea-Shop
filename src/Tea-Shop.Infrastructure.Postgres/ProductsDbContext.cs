using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Domain.Tokens;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres;

public sealed class ProductsDbContext: DbContext, IReadDbContext
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
        modelBuilder.HasPostgresExtension("ltree");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Basket> Buskets { get; set; }

    public DbSet<BasketItem> BusketsItems { get; set; }

    public DbSet<Kit> Kits { get; set; }

    public DbSet<KitDetails> KitDetails { get; set; }

    public DbSet<KitItem> KitsItems { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<ProductsTags> ProductsTags { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }


    public IQueryable<Product> ProductsRead => Set<Product>().AsNoTracking();

    public IQueryable<Order> OrdersRead => Set<Order>().AsNoTracking();

    public IQueryable<Comment> CommentsRead => Set<Comment>().AsNoTracking();

    public IQueryable<Review> ReviewsRead => Set<Review>().AsNoTracking();

    public IQueryable<User> UsersRead => Set<User>().AsNoTracking();

    public IQueryable<Kit> KitsRead => Set<Kit>().AsNoTracking();

    public IQueryable<Basket> BusketsRead => Set<Basket>().AsNoTracking();

    public IQueryable<BasketItem> BusketsItemsRead => Set<BasketItem>().AsNoTracking();
}