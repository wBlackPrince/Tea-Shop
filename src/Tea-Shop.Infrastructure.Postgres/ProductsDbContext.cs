using Microsoft.EntityFrameworkCore;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres;

public class ProductsDbContext: DbContext
{
    public DbSet<Product> Products { get; set; }
}