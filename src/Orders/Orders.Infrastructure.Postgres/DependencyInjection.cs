using Microsoft.Extensions.DependencyInjection;
using Orders.Application;
using Orders.Infrastructure.Postgres.Database;
using Orders.Infrastructure.Postgres.Repositories;
using Shared.Database;

namespace Orders.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddScoped<IOrdersRepository, OrdersRepository>();

        services.AddSingleton<ISqlConnectionFactory, OrdersSqlConnectionFactory>();
        services.AddSingleton<IDbConnectionFactory, OrdersNpgSqlConnectionFactory>();
        services.AddScoped<ITransactionManager, OrdersTransactionManager>();

        return services;
    }
}