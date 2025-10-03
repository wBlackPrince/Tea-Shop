using Microsoft.Extensions.DependencyInjection;
using Users.Application;
using Users.Contracts;
using Users.Infrastructure.Postgres;

namespace Users.Controllers;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersDependencies(this IServiceCollection services)
    {
        services.AddPostgresDependencies();
        services.AddUsersApplication();

        services.AddScoped<IUsersContracts, UsersContracts>();

        return services;
    }
}