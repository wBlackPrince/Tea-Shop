using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Tea_Shop.Application;
using Tea_Shop.Application.Database;
using Tea_Shop.Infrastructure.Postgres;

namespace Tea_Shop.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddWebDependencies();
        services.AddPostgresDependencies();

        return services;
    }

    private static IServiceCollection AddWebDependencies(
        this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson();
        services.AddOpenApi();

        services.AddControllers(options =>
        {
            options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
        });

        return services;
    }
}

public static class MyJPIF
{
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        var builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        return builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}