using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Orders.Contracts.Dtos;
using Shared.Abstractions;
using Users.Application.Commands.AddBasketItemCommand;
using Users.Application.Commands.CreateBasketCommand;
using Users.Application.Commands.CreateUserCommand;
using Users.Application.Commands.DeleteUserCommand;
using Users.Application.Commands.LoginUserCommand;
using Users.Application.Commands.LoginUserWithRefreshTokenCommand;
using Users.Application.Commands.RemoveBasketItemCommand;
using Users.Application.Commands.RevokeRefreshTokensCommand;
using Users.Application.Commands.UpdateBasketItemCommand;
using Users.Application.Commands.UpdateUserCommand;
using Users.Application.Commands.VerifyEmailCommand;
using Users.Application.Queries.GetBasketByIdQuery;
using Users.Application.Queries.GetBasketItemByIdQuery;
using Users.Application.Queries.GetUserByIdQuery;
using Users.Application.Queries.GetUserCommentsQuery;
using Users.Application.Queries.GetUserOrdersQuery;
using Users.Application.Queries.GetUserReviewsQuery;
using Users.Application.Queries.GetUsersQuery;
using Users.Contracts.Dtos;

namespace Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUsersApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ICommandHandler<AddBasketItemDto?, AddBasketItemCommand>, AddBasketItemHandler>();
        services.AddScoped<ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand>, RemoveBasketItemHandler>();
        services.AddScoped<ICommandHandler<BasketDto, CreateBasketCommand>, CreateBasketHandler>();
        services.AddScoped<UpdateBasketItemHandler>();
        services.AddScoped<IQueryHandler<BasketDto?, GetBasketByIdQuery>, GetBasketByIdHandler>();
        services.AddScoped<IQueryHandler<BasketItemDto?, GetBasketItemByIdQuery>, GetBasketItemHandler>();

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

        services.AddScoped<EmailVerificationLinkFactory>();

        return services;
    }
}