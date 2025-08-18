using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Infrastructure.Postgres.Repositories;

namespace Tea_Shop.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ProductsDbContext>();
        services.AddScoped<IProductsRepository, ProductsEfCoreRepository>();
        // выбираем для DI либо ef core либо postgres
        //services.AddScoped<IProductsRepository, ProductsSqlRepository>();

        return services;
    }
}