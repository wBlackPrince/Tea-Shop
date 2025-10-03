using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Commands.CreateOrderCommand;
using Orders.Application.Commands.DeleteOrderCommand;
using Orders.Application.Commands.UpdateOrderCommand;
using Orders.Application.Queries.GetOrderByIdQuery;
using Orders.Application.Queries.GetOrderItemsQuery;
using Orders.Contracts.Dtos;
using Shared.Abstractions;

namespace Orders.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrdersApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

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

        return services;
    }
}