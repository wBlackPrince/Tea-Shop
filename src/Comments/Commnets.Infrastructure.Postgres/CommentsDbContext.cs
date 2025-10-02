using Comments.Application;
using Comments.Domain;
using Microsoft.EntityFrameworkCore;

namespace Commnets.Infrastructure.Postgres;

public class CommentsDbContext: DbContext, ICommentsReadDbContext
{
    private readonly string _connectionString;

    public CommentsDbContext()
    {
    }

    public CommentsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentsDbContext).Assembly);
    }

    public DbSet<Comment> Comments { get; set; }

    public IQueryable<Comment> CommentsRead => Set<Comment>().AsNoTracking();
}