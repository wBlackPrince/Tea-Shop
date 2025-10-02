using Products.Domain;

namespace Products.Application;

public interface IProductsReadDbContext
{
    public IQueryable<Product> ProductsRead { get; }
}