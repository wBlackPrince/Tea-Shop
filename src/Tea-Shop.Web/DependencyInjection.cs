using Tea_Shop.Application;

namespace Tea_Shop.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddWebDependencies();

        return services;
    }

    private static IServiceCollection AddWebDependencies(
        this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}