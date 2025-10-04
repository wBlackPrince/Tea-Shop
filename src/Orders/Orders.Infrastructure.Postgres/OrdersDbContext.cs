using Microsoft.EntityFrameworkCore;
using Orders.Application;
using Orders.Domain;

namespace Orders.Infrastructure.Postgres;

public class OrdersDbContext: DbContext, IOrdersReadDbContext
{
    private readonly string _connectionString;

    public OrdersDbContext()
    {
    }

    public OrdersDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
    }

    public DbSet<Order> Orders { get; set; }

    public IQueryable<Order> OrdersRead => Set<Order>().AsNoTracking();
}