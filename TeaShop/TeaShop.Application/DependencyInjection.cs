using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using TeaShop.Application.Products;

namespace TeaShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}