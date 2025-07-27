using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TeaShop.Application.Database;

namespace TeaShop.Infrastructure.Postgres;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(
            _configuration.GetConnectionString("DatabaseConnection"));

        return connection;
    }
}