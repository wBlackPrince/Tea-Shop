using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Auth;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Social;
using Tea_Shop.Application.Users;
using Tea_Shop.Infrastructure.Postgres.Auth;
using Tea_Shop.Infrastructure.Postgres.Database;
using Tea_Shop.Infrastructure.Postgres.Repositories;
using Tea_Shop.Infrastructure.Postgres.Seeders;

namespace Tea_Shop.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IProductsRepository, ProductsRepository>();

        // выбираем для DI либо ef core либо postgres

        // services.AddScoped<IProductsRepository, ProductsSqlRepository>();

        services.AddScoped<IOrdersRepository, OrdersRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<ISocialRepository, SocialRepository>();
        services.AddScoped<ITokensRepository, TokensRepository>();
        services.AddScoped<ISeeder, ProductsSeeders>();

        services.AddSingleton<IDbConnectionFactory, NpgSqlConnectionFactory>();

        services.AddScoped<ITransactionManager, TransactionManager>();

        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}