using TeaShop.Application;
using TeaShop.Infrastructure.Postgres;

namespace TeaShop.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddWebDependencies();
        services.AddPostgresInfrastructure();

        return services;
    }

    private static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        return services;

    }
}