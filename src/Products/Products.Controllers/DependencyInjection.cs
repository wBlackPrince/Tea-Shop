using Microsoft.Extensions.DependencyInjection;
using Products.Application;
using Products.Contracts;
using Products.Infrastructure.Postgres;

namespace Products.Controllers;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsDependencies(this IServiceCollection services)
    {
        services.AddPostgresDependencies();
        services.AddProductsApplication();
        services.AddScoped<IProductsContracts, ProductsContracts>();

        return services;
    }
}