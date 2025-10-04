using Microsoft.EntityFrameworkCore;
using Users.Application;
using Users.Domain;

namespace Users.Infrastructure.Postgres;

public class UsersDbContext: DbContext, IUsersReadDbContext
{
    private readonly string _connectionString;

    public UsersDbContext()
    {
    }

    public UsersDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("users");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
    }

    public DbSet<User> Users { get; set; }

    public IQueryable<User> UsersRead => Set<User>().AsNoTracking();

    public DbSet<Basket> Baskets { get; set; }

    public DbSet<BasketItem> BasketItems { get; set; }

    public IQueryable<Basket> BasketsRead => Set<Basket>().AsNoTracking();

    public IQueryable<BasketItem> BasketsItemsRead => Set<BasketItem>().AsNoTracking();

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
}