using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Tea_Shop.Infrastructure.Postgres;

namespace Tea_Shop.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        //services.AddApplication();
        services.AddWebDependencies();
        //services.AddPostgresDependencies();
        services.AddSwaggerGenWithAuthentification();

        return services;
    }

    private static IServiceCollection AddWebDependencies(
        this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson();
        // services.AddOpenApi();

        services.AddControllers(options =>
        {
            options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
        });

        return services;
    }

    private static IServiceCollection AddSwaggerGenWithAuthentification(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace("+", "-"));

            var securitySchema = new OpenApiSecurityScheme
            {
                Name = "Jwt Authentication",
                Description = "Enter your JWT token in this field",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
            };

            o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme,
                        }
                    },
                    []
                }
            };

            o.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}