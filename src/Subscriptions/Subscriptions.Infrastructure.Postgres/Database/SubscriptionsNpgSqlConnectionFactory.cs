using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Shared.Database;

namespace Subscriptions.Infrastructure.Postgres.Database;


public class SubscriptionsNpgSqlConnectionFactory: IDisposable, IAsyncDisposable, IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;

    public SubscriptionsNpgSqlConnectionFactory(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(
            configuration.GetConnectionString("TeaShopDb"));

        dataSourceBuilder
            .UseLoggerFactory(CreateLoggerFactory());

        _dataSource = dataSourceBuilder.Build();
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken)
    {
        return await _dataSource.OpenConnectionAsync();
    }

    private ILoggerFactory CreateLoggerFactory()
        => LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}