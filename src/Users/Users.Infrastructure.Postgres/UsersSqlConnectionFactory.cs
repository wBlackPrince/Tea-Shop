using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Database;

namespace Users.Infrastructure.Postgres;

public class UsersSqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public UsersSqlConnectionFactory(IConfiguration configuration)
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