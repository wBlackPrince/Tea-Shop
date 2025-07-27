using Microsoft.EntityFrameworkCore;
using TeaShopDomain.Products;

namespace TeaShop.Infrastructure.Postgres;

public class ProductsDbContext: DbContext
{
    public DbSet<Product> Products { get; set; }
}