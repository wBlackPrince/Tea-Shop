using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Comments;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Orders.Commands;
using Tea_Shop.Application.Orders.Queries;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Products.Commands;
using Tea_Shop.Application.Products.Queries;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Tags;
using Tea_Shop.Application.Users;

namespace Tea_Shop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ITagsService, TagsService>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IReviewsService, ReviewsService>();
        services.AddScoped<ICommentsService, CommentsService>();

        // handlers для продуктов
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<GetProductIngredientsHandler>();
        services.AddScoped<GetProductsByTagHandler>();

        // handlers для заказов
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<DeleteOrderHandler>();
        services.AddScoped<GetOrderByIdHandler>();

        return services;
    }
}