using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Shared.Database;

namespace Commnets.Infrastructure.Postgres;

public class CommentsSqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public CommentsSqlConnectionFactory(IConfiguration configuration)
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