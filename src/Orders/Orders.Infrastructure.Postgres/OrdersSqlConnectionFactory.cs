using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Database;

namespace Orders.Infrastructure.Postgres;

public class OrdersSqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public OrdersSqlConnectionFactory(IConfiguration configuration)
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