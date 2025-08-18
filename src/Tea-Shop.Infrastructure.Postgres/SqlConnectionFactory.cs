using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Tea_Shop.Application;
using Tea_Shop.Application.Database;

namespace Tea_Shop.Infrastructure.Postgres;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
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