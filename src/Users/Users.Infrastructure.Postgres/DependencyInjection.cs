using Microsoft.Extensions.DependencyInjection;
using Shared.Database;
using Users.Application;
using Users.Infrastructure.Postgres.Database;
using Users.Infrastructure.Postgres.Repositories;

namespace Users.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IBasketsRepository, BasketsRepository>();
        services.AddScoped<ITokensRepository, TokensRepository>();

        services.AddSingleton<ISqlConnectionFactory, UsersSqlConnectionFactory>();
        services.AddSingleton<IDbConnectionFactory, UsersNpgSqlConnectionFactory>();
        services.AddScoped<ITransactionManager, UsersTransactionManager>();

        services.AddSingleton<ITokenProvider, TokenProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}