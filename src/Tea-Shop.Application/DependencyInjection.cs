using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Tea_Shop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IProductsService, ProductsService>();

        return services;
    }
}