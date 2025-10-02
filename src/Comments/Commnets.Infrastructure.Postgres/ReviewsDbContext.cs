using Comments.Application;
using Comments.Domain;
using Microsoft.EntityFrameworkCore;

namespace Commnets.Infrastructure.Postgres;

public class ReviewsDbContext: DbContext, IReviewsReadDbContext
{
    private readonly string _connectionString;

    public ReviewsDbContext()
    {
    }

    public ReviewsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReviewsDbContext).Assembly);
    }

    public DbSet<Review> Reviews { get; set; }

    public IQueryable<Review> ReviewsRead => Set<Review>().AsNoTracking();
}