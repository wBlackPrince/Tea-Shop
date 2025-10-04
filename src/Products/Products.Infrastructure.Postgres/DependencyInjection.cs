using Microsoft.Extensions.DependencyInjection;
using Products.Application;
using Products.Infrastructure.Postgres.Database;
using Products.Infrastructure.Postgres.Repositories;
using Shared.Database;

namespace Products.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddScoped<IProductsRepository, ProductsRepository>();

        services.AddSingleton<ISqlConnectionFactory, ProductsSqlConnectionFactory>();
        services.AddSingleton<IDbConnectionFactory, ProductsNpgSqlConnectionFactory>();
        services.AddScoped<ITransactionManager, ProductsTransactionManager>();

        return services;
    }
}