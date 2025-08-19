using Dapper;
using Tea_Shop.Application.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class ProductsSqlRepository: IProductsRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public ProductsSqlRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Guid> GetProduct(Guid productId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM products WHERE id = @Id";

        using var connection = _sqlConnectionFactory.CreateConnection();

        var getProductParams = new { Id = productId };

        await connection.ExecuteAsync(sql, getProductParams);

        return productId;
    }

    public async Task<Guid> CreateProduct(Product product, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }

    public async Task<Guid> UpdateProductPrice(Guid productId, float price, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE price SET name = @Price FROM products WHERE id = @Id";

        using var connection = _sqlConnectionFactory.CreateConnection();

        var updateProductPriceParams = new { Id = productId,  Price = price };

        await connection.ExecuteAsync(sql, updateProductPriceParams);

        return productId;
    }

    public async Task<Guid> DeleteProduct(Guid productId, CancellationToken cancellationToken)
    {
        const string sql = "DELETE FROM products WHERE id = @Id";

        using var connection = _sqlConnectionFactory.CreateConnection();

        var deleteProductParams = new { Id = productId };

        await connection.ExecuteAsync(sql, deleteProductParams);

        return productId;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}