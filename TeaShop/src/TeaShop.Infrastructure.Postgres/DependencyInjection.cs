using Microsoft.Extensions.DependencyInjection;
using TeaShop.Application.Comments;
using TeaShop.Application.Database;
using TeaShop.Application.Products;
using TeaShop.Application.Reviews;
using TeaShop.Application.Tags;

namespace TeaShop.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ProductsDbContext>();

        services.AddScoped<IProductsRepository, ProductsEfCoreRepository>();
        services.AddScoped<ICommentsRepository, CommentsEfCoreRepository>();
        services.AddScoped<IReviewsRepository, ReviewsEfCoreRepository>();
        services.AddScoped<ITagsRepository, TagsEfCoreRepository>();

        return services;
    }
}