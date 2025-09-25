using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Auth;
using Tea_Shop.Application.Baskets.Commands.AddBasketItemCommand;
using Tea_Shop.Application.Baskets.Commands.RemoveBasketItemCommand;
using Tea_Shop.Application.Comments.Commands.CreateCommentCommand;
using Tea_Shop.Application.Comments.Commands.DeleteCommentCommand;
using Tea_Shop.Application.Comments.Commands.UpdateCommentCommand;
using Tea_Shop.Application.Comments.Queries.GetCommentByIdQuery;
using Tea_Shop.Application.Comments.Queries.GetCommentChildCommentsQuery;
using Tea_Shop.Application.Orders.Commands.CreateOrderCommand;
using Tea_Shop.Application.Orders.Commands.DeleteOrderCommand;
using Tea_Shop.Application.Orders.Commands.UpdateOrderCommand;
using Tea_Shop.Application.Orders.Queries.GetOrderByIdQuery;
using Tea_Shop.Application.Orders.Queries.GetOrderItemsQuery;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Application.Products.Commands.DeleteProductCommand;
using Tea_Shop.Application.Products.Commands.UpdatePreparationDescription;
using Tea_Shop.Application.Products.Commands.UpdatePreparationTime;
using Tea_Shop.Application.Products.Commands.UpdateProductCommand;
using Tea_Shop.Application.Products.Commands.UpdateProductIngredients;
using Tea_Shop.Application.Products.Queries.GetPopularProductsQuery;
using Tea_Shop.Application.Products.Queries.GetProductByIdQuery;
using Tea_Shop.Application.Products.Queries.GetProductIngredientsQuery;
using Tea_Shop.Application.Products.Queries.GetProductReviews;
using Tea_Shop.Application.Products.Queries.GetProductsQuery;
using Tea_Shop.Application.Products.Queries.GetSimilarProductsQuery;
using Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;
using Tea_Shop.Application.Reviews.Commands.DeleteReviewCommand;
using Tea_Shop.Application.Reviews.Commands.UpdateReviewCommand;
using Tea_Shop.Application.Reviews.Queries.GetReviewByIdQuery;
using Tea_Shop.Application.Reviews.Queries.GetReviewCommentsQuery;
using Tea_Shop.Application.Tags;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.LoginUserCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Queries.GetUserByIdQuery;
using Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;
using Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;
using Tea_Shop.Application.Users.Queries.GetUserReviewsQuery;
using Tea_Shop.Application.Users.Queries.GetUsersQuery;
using Tea_Shop.Contract.Baskets;
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
            ICommandHandler<ProductWithOnlyIdDto, UpdateProductIngredientsCommand>,
            UpdateProductIngredientsHandler>();
        services.AddScoped<
            ICommandHandler<DeleteProductDto, DeleteProductQuery>,
            DeleteProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<
            ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationDescriptionCommand>,
            UpdatePreparationDescriptionHandler>();
        services.AddScoped<
            ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationTimeCommand>,
            UpdatePreparationTimeHandler>();
        services.AddScoped<
            IQueryHandler<GetProductDto?, GetProductByIdQuery>,
            GetProductByIdHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<
            IQueryHandler<GetIngrendientsResponseDto[], GetProductsIngredientsQuery>,
            GetProductIngredientsHandler>();
        services.AddScoped<
            IQueryHandler<GetProductsResponseDto, GetProductsQuery>,
            GetProductsHandler>();
        services.AddScoped<
            IQueryHandler<GetReviewResponseDto[], GetProductReviewsQuery>,
            GetProductReviewsHandler>();
        services.AddScoped<
            IQueryHandler<GetPopularProductsResponseDto[], GetPopularProductsQuery>,
            GetPopularProductsHandler>();
        services.AddScoped<
            IQueryHandler<GetSimilarProductResponseDto[], GetSimilarProductsQuery>,
            GetSimilarProductsHandler>();

        // handlers для заказов
        services.AddScoped<
            IQueryHandler<GetOrderResponseDto?, GetOrderByIdQuery>,
            GetOrderByIdHandler>();
        services.AddScoped<
            IQueryHandler<OrderItemResponseDto[], GetOrderItemQuery>,
            GetOrderItemsHandler>();
        services.AddScoped<
            ICommandHandler<CreateOrderResponseDto, CreateOrderCommand>,
            CreateOrderHandler>();
        services.AddScoped<UpdateOrderHandler>();
        services.AddScoped<
            ICommandHandler<DeleteOrderDto, DeleteOrderCommand>,
            DeleteOrderHandler>();

        // handlers для корзин
        services.AddScoped<ICommandHandler<AddBasketItemDto?, AddBasketItemCommand>, AddBasketItemHandler>();
        services.AddScoped<ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand>, RemoveBasketItemHandler>();

        // handlers для комментов
        services.AddScoped<
            IQueryHandler<CommentDto?, GetCommentByIdQuery>,
            GetCommentByIdHandler>();
        services.AddScoped<
            IQueryHandler<GetChildCommentsResponseDto, GetCommentChildCommentsQuery>,
            GetCommentChildCommentsHandler>();
        services.AddScoped<
            ICommandHandler<CreateCommentResponseDto, CreateCommentCommand>,
            CreateCommentHandler>();
        services.AddScoped<UpdateCommentHandler>();
        services.AddScoped<
            ICommandHandler<CommentWithOnlyIdDto, DeleteCommentCommand>,
            DeleteCommentHandler>();

        // handlers для обзоров
        services.AddScoped<
            IQueryHandler<GetReviewResponseDto?, GetReviewByIdQuery>,
            GetReviewByIdHandler>();
        services.AddScoped<
            IQueryHandler<GetReviewCommentsResponseDto, GetReviewCommentsQuery>,
            GetReviewCommentsHandler>();
        services.AddScoped<
            ICommandHandler<CreateReviewResponseDto, CreateReviewCommand>,
            CreateReviewHandler>();
        services.AddScoped<UpdateReviewHandler>();
        services.AddScoped<
            ICommandHandler<DeleteReviewDto, DeleteReviewCommand>,
            DeleteReviewHandler>();

        // handlers для пользователей
        services.AddScoped<
            ICommandHandler<CreateUserResponseDto, CreateUserCommand>,
            CreateUserHandler>();
        services.AddScoped<
            ICommandHandler<UserWithOnlyIdDto, DeleteUserCommand>,
            DeleteUserHandler>();
        services.AddScoped<
            IQueryHandler<GetUsersResponseDto, GetUsersQuery>,
            GetUsersHandler>();
        services.AddScoped<
            IQueryHandler<GetUserResponseDto?, GetUserByIdQuery>,
            GetUserByIdHandler>();
        services.AddScoped<
            IQueryHandler<GetUserOrdersResponseDto?, GetUserOrdersQuery>,
            GetUserOrdersHandler>();
        services.AddScoped<
            IQueryHandler<GetUserCommentsResponseDto?, GetUserCommentsQuery>,
            GetUserCommentsHandler>();
        services.AddScoped<
            IQueryHandler<GetUserReviewsResponseDto?, GetUserReviewsQuery>,
            GetUserReviewsHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<
            ICommandHandler<string, LoginUserCommand>,
            LoginUserHandler>();

        return services;
    }
}