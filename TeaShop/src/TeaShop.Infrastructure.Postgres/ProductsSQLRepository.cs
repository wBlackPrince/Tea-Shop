using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TeaShop.Application.Database;
using TeaShopDomain.Products;

namespace TeaShop.Infrastructure.Postgres;

public class ProductsSQLRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public ProductsSQLRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Guid> AddAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        const string sql =
            """  
            INSERT INTO products (id, title, price, amount, description, tags, photos)
            Values (@Id, @Title, @Price, @Amount, @Description, @Tags, @Photos)
            """;

        using var connection = _sqlConnectionFactory.CreateConnection();

        await connection.ExecuteAsync(sql,
            new
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Amount = product.Amount,
                Description = product.Description,
                TagsIds = product.TagsIds,
                PhotosIds = product.PhotosIds,
            });

        return product.Id;
    }
}