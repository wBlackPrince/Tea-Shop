using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Comments;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Tags;
using Tea_Shop.Application.Users;
using Tea_Shop.Infrastructure.Postgres.Repositories;
using Tea_Shop.Infrastructure.Postgres.Seeders;

namespace Tea_Shop.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IProductsRepository, ProductsEfCoreRepository>();
        // выбираем для DI либо ef core либо postgres
        //services.AddScoped<IProductsRepository, ProductsSqlRepository>();

        services.AddScoped<ITagsRepository, TagsEfCoreRepository>();
        services.AddScoped<IUsersRepository, UsersEfCoreRepository>();
        services.AddScoped<IReviewsRepository, ReviewsEfCoreRepository>();
        services.AddScoped<ICommentsRepository, CommentsEfCoreRepository>();
        services.AddScoped<ISeeder, ProductsSeeders>();

        return services;
    }
}