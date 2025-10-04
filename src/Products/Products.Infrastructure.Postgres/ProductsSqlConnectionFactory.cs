using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Database;

namespace Products.Infrastructure.Postgres;

public class ProductsSqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public ProductsSqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(
            _configuration.GetConnectionString("PostgresConnection"));

        return connection;
    }
}