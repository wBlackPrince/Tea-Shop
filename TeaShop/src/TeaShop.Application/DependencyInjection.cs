using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using TeaShop.Application.Comments;
using TeaShop.Application.Products;
using TeaShop.Application.Reviews;
using TeaShop.Application.Tags;

namespace TeaShop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<ITagsService, TagsService>();

        return services;
    }
}