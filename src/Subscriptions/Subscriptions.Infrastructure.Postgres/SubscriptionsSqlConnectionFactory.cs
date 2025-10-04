using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Database;

namespace Subscriptions.Infrastructure.Postgres;

public class SubscriptionsSqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SubscriptionsSqlConnectionFactory(IConfiguration configuration)
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