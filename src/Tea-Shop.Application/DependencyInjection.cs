using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Auth;
using Tea_Shop.Application.EmailVerification;
using Tea_Shop.Application.Orders.Commands.CreateOrderCommand;
using Tea_Shop.Application.Orders.Commands.DeleteOrderCommand;
using Tea_Shop.Application.Orders.Commands.UpdateOrderCommand;
using Tea_Shop.Application.Orders.Queries.GetOrderByIdQuery;
using Tea_Shop.Application.Orders.Queries.GetOrderItemsQuery;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Application.Products.Commands.CreateTagCommand;
using Tea_Shop.Application.Products.Commands.DeleteProductCommand;
using Tea_Shop.Application.Products.Commands.DeleteTagCommand;
using Tea_Shop.Application.Products.Commands.UpdatePreparationDescription;
using Tea_Shop.Application.Products.Commands.UpdatePreparationTime;
using Tea_Shop.Application.Products.Commands.UpdateProductCommand;
using Tea_Shop.Application.Products.Commands.UpdateProductIngredients;
using Tea_Shop.Application.Products.Commands.UploadProductsPhotosCommand;
using Tea_Shop.Application.Products.Queries.GetPopularProductsQuery;
using Tea_Shop.Application.Products.Queries.GetProductByIdQuery;
using Tea_Shop.Application.Products.Queries.GetProductIngredientsQuery;
using Tea_Shop.Application.Products.Queries.GetProductReviews;
using Tea_Shop.Application.Products.Queries.GetProductsQuery;
using Tea_Shop.Application.Products.Queries.GetSimilarProductsQuery;
using Tea_Shop.Application.Social.Commands.CreateCommentCommand;
using Tea_Shop.Application.Social.Commands.CreateReviewCommand;
using Tea_Shop.Application.Social.Commands.DeleteCommentCommand;
using Tea_Shop.Application.Social.Commands.DeleteReviewCommand;
using Tea_Shop.Application.Social.Commands.UpdateCommentCommand;
using Tea_Shop.Application.Social.Commands.UpdateReviewCommand;
using Tea_Shop.Application.Social.Queries.GetCommentByIdQuery;
using Tea_Shop.Application.Social.Queries.GetCommentChildCommentsQuery;
using Tea_Shop.Application.Social.Queries.GetDescendantsQuery;
using Tea_Shop.Application.Social.Queries.GetHierarchyQuery;
using Tea_Shop.Application.Social.Queries.GetNeighboursQuery;
using Tea_Shop.Application.Social.Queries.GetReviewByIdQuery;
using Tea_Shop.Application.Social.Queries.GetReviewCommentsQuery;
using Tea_Shop.Application.Users;
using Tea_Shop.Application.Users.Commands.AddBasketItemCommand;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.LoginUserCommand;
using Tea_Shop.Application.Users.Commands.LoginUserWithRefreshTokenCommand;
using Tea_Shop.Application.Users.Commands.RemoveBasketItemCommand;
using Tea_Shop.Application.Users.Commands.RevokeRefreshTokensCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Commands.VerifyEmailCommand;
using Tea_Shop.Application.Users.Queries.GetBasketByIdQuery;
using Tea_Shop.Application.Users.Queries.GetBasketItemByIdQuery;
using Tea_Shop.Application.Users.Queries.GetUserByIdQuery;
using Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;
using Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;
using Tea_Shop.Application.Users.Queries.GetUserReviewsQuery;
using Tea_Shop.Application.Users.Queries.GetUsersQuery;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Contract.Products;
using Tea_Shop.Contract.Social;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // handlers для продуктов
        services.AddScoped<
            ICommandHandler<CreateProductResponseDto, CreateProductCommand>,
            CreateProductHandler>();
        services.AddScoped<
            ICommandHandler<Guid, CreateTagCommand>,
            CreateTagHandler>();
        services.AddScoped<
            ICommandHandler<Guid, UploadProductsPhotosCommand>,
            UploadProductsPhotosHandler>();
        services.AddScoped<
            ICommandHandler<ProductWithOnlyIdDto, UpdateProductIngredientsCommand>,
            UpdateProductIngredientsHandler>();
        services.AddScoped<
            ICommandHandler<DeleteProductDto, DeleteProductQuery>,
            DeleteProductHandler>();
        services.AddScoped<
            ICommandHandler<Guid, DeleteTagCommand>,
            DeleteTagHandler>();
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

        // handlers для комментов
        services.AddScoped<
            IQueryHandler<CommentDto?, GetCommentByIdQuery>,
            GetCommentByIdHandler>();
        services.AddScoped<
            IQueryHandler<CommentsResponseDto, GetHierarchyQuery>,
            GetHierarchyHandler>();
        services.AddScoped<
            IQueryHandler<CommentsResponseDto, GetNeighboursQuery>,
            GetNeighboursHandler>();
        services.AddScoped<
            IQueryHandler<CommentsResponseDto, GetDescendantsQuery>,
            GetDescendantsHandler>();
        services.AddScoped<
            IQueryHandler<CommentsResponseDto, GetCommentChildCommentsQuery>,
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
            ICommandHandler<LoginResponseDto, LoginUserCommand>,
            LoginUserHandler>();
        services.AddScoped<
            ICommandHandler<LoginResponseDto, LoginUserWithRefreshTokenCommand>,
            LoginUserWithRefreshTokenHandler>();
        services.AddScoped<RevokeRefreshTokensHandler>();
        services.AddScoped<VerifyEmail>();
        services.AddScoped<
            ICommandHandler<AddBasketItemDto?, AddBasketItemCommand>,
            AddBasketItemHandler>();
        services.AddScoped<
            ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand>,
            RemoveBasketItemHandler>();
        services.AddScoped<
            IQueryHandler<BasketDto?, GetBasketByIdQuery>,
            GetBasketByIdHandler>();
        services.AddScoped<
            IQueryHandler<BasketItemDto?, GetBasketItemByIdQuery>,
            GetBasketItemByIdHandler>();

        services.AddScoped<EmailVerificationLinkFactory>();

        return services;
    }
}