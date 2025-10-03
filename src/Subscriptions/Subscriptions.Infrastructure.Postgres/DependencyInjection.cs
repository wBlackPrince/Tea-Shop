using Microsoft.Extensions.DependencyInjection;
using Shared.Database;
using Subscriptions.Infrastructure.Postgres.Database;

namespace Subscriptions.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SubscriptionsSqlConnectionFactory>();
        services.AddSingleton<IDbConnectionFactory, SubscriptionsNpgSqlConnectionFactory>();
        services.AddScoped<ITransactionManager, SubscriptionsTransactionManager>();

        return services;
    }
}