using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Comments;
using Tea_Shop.Application.Comments.Commands;
using Tea_Shop.Application.Comments.Queries;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Orders.Commands;
using Tea_Shop.Application.Orders.Queries;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Products.Commands;
using Tea_Shop.Application.Products.Queries;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Reviews.Commands;
using Tea_Shop.Application.Reviews.Queries;
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

        // handlers для продуктов
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<GetProductIngredientsHandler>();
        services.AddScoped<GetProductsByTagHandler>();

        // handlers для заказов
        services.AddScoped<GetOrderByIdHandler>();
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<UpdateOrderHandler>();
        services.AddScoped<DeleteOrderHandler>();

        // handlers для комментов
        services.AddScoped<GetCommentByIdHandler>();
        services.AddScoped<CreateCommentHandler>();
        services.AddScoped<UpdateCommentHandler>();
        services.AddScoped<DeleteCommentHandler>();

        // handlers для обзоров
        services.AddScoped<GetReviewByIdHandler>();
        services.AddScoped<CreateReviewHandler>();
        services.AddScoped<UpdateReviewHandler>();
        services.AddScoped<DeleteReviewHandler>();

        return services;
    }
}