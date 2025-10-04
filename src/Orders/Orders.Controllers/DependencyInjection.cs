using Microsoft.Extensions.DependencyInjection;
using Orders.Application;
using Orders.Infrastructure.Postgres;

namespace Orders.Controllers;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersDependencies(this IServiceCollection services)
    {
        services.AddPostgresDependencies();
        services.AddOrdersApplication();

        return services;
    }
}