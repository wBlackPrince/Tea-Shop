using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Comments.Commands.CreateCommentCommand;
using Tea_Shop.Application.Comments.Commands.DeleteCommentCommand;
using Tea_Shop.Application.Comments.Commands.UpdateCommentCommand;
using Tea_Shop.Application.Comments.Queries;
using Tea_Shop.Application.Orders.Commands.CreateOrderCommand;
using Tea_Shop.Application.Orders.Commands.DeleteOrderCommand;
using Tea_Shop.Application.Orders.Commands.UpdateOrderCommand;
using Tea_Shop.Application.Orders.Queries.GetOrderByIdQuery;
using Tea_Shop.Application.Orders.Queries.GetOrderItemsQuery;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Application.Products.Commands.DeleteProductCommand;
using Tea_Shop.Application.Products.Commands.UpdateProductCommand;
using Tea_Shop.Application.Products.Queries.GetPopularProductsQuery;
using Tea_Shop.Application.Products.Queries.GetProductByIdQuery;
using Tea_Shop.Application.Products.Queries.GetProductIngredientsQuery;
using Tea_Shop.Application.Products.Queries.GetProductReviews;
using Tea_Shop.Application.Products.Queries.GetProductsByTagQuery;
using Tea_Shop.Application.Products.Queries.GetSeasonalProductsQuery;
using Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;
using Tea_Shop.Application.Reviews.Commands.DeleteReviewCommand;
using Tea_Shop.Application.Reviews.Commands.UpdateReviewCommand;
using Tea_Shop.Application.Reviews.Queries;
using Tea_Shop.Application.Tags;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Queries;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Contract.Products;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ITagsService, TagsService>();

        // handlers для продуктов
        services.AddScoped<
            ICommandHandler<CreateProductResponseDto, CreateProductCommand>,
            CreateProductHandler>();
        services.AddScoped<
            ICommandHandler<DeleteProductDto, DeleteProductQuery>,
            DeleteProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<
            IQueryHandler<GetProductDto?, GetProductByIdQuery>,
            GetProductByIdHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<
            IQueryHandler<GetIngrendientsResponseDto[], GetProductsIngredientsQuery>,
            GetProductIngredientsHandler>();
        services.AddScoped<
            IQueryHandler<GetProductDto[], GetProductsByTagQuery>,
            GetProductsByTagHandler>();
        services.AddScoped<
            IQueryHandler<GetReviewDto[], GetProductReviewsQuery>,
            GetProductReviewsHandler>();
        services.AddScoped<
            IQueryHandler<GetPopularProductsResponseDto[], GetPopularProductsQuery>,
            GetPopularProductsHandler>();
        services.AddScoped<
            IQueryHandler<GetSimpleProductResponseDto[], GetSeasonalProductsQuery>,
            GetSeasonalProductsHandler>();

        // handlers для заказов
        services.AddScoped<
            IQueryHandler<GetOrderResponseDto?, GetOrderByIdQuery>,
            GetOrderByIdHandler>();
        services.AddScoped<
            IQueryHandler<OrderItemDto[], GetOrderItemQuery>,
            GetOrderItemsHandler>();
        services.AddScoped<
            ICommandHandler<CreateOrderResponseDto, CreateOrderCommand>,
            CreateOrderHandler>();
        services.AddScoped<UpdateOrderHandler>();
        services.AddScoped<
            ICommandHandler<DeleteOrderDto, DeleteOrderCommand>,
            DeleteOrderHandler>();

        // handlers для комментов
        services.AddScoped<GetCommentByIdHandler>();
        services.AddScoped<
            ICommandHandler<CreateCommentResponseDto, CreateCommentCommand>,
            CreateCommentHandler>();
        services.AddScoped<UpdateCommentHandler>();
        services.AddScoped<DeleteCommentHandler>();

        // handlers для обзоров
        services.AddScoped<
            GetReviewByIdHandler>();
        services.AddScoped<
            ICommandHandler<CreateReviewResponseDto, CreateReviewCommand>,
            CreateReviewHandler>();
        services.AddScoped<UpdateReviewHandler>();
        services.AddScoped<DeleteReviewHandler>();

        // handlers для пользователей
        services.AddScoped<
            ICommandHandler<CreateUserResponseDto, CreateUserCommand>,
            CreateUserHandler>();
        services.AddScoped<DeleteUserHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<GetUserByIdHandler>();
        services.AddScoped<GetActiveUsersHandler>();
        services.AddScoped<GetBannedUsersHandler>();

        return services;
    }
}