using Microsoft.EntityFrameworkCore;
using Subscriptions.Application;
using Subscriptions.Domain;

namespace Subscriptions.Infrastructure.Postgres;

public class SubscriptionsDbContext: DbContext, ISubscriptionsReadDbContext
{
    private readonly string _connectionString;

    public SubscriptionsDbContext()
    {
    }

    public SubscriptionsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriptionsDbContext).Assembly);
    }

    public DbSet<Kit> Kits { get; set; }

    public DbSet<KitDetails> KitDetails { get; set; }

    public DbSet<KitItem> KitsItems { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public IQueryable<Kit> KitsRead => Set<Kit>().AsNoTracking();
}