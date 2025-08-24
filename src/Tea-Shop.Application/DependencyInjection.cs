using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Comments;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Tags;
using Tea_Shop.Application.Users;

namespace Tea_Shop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<ITagsService, TagsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<ICommentsService, CommentsService>();

        return services;
    }
}