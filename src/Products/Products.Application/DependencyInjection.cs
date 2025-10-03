using Comments.Contracts.Dtos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Commands.CreateProductCommand;
using Products.Application.Commands.DeleteProductCommand;
using Products.Application.Commands.UpdatePreparationDescription;
using Products.Application.Commands.UpdatePreparationTime;
using Products.Application.Commands.UpdateProductCommand;
using Products.Application.Commands.UpdateProductIngredients;
using Products.Application.Commands.UploadProductsPhotosCommand;
using Products.Application.Queries.GetPopularProductsQuery;
using Products.Application.Queries.GetProductByIdQuery;
using Products.Application.Queries.GetProductIngredientsQuery;
using Products.Application.Queries.GetProductReviews;
using Products.Application.Queries.GetProductsQuery;
using Products.Application.Queries.GetSimilarProductsQuery;
using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddProductsApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // handlers для продуктов
        services.AddScoped<
            ICommandHandler<CreateProductResponseDto, CreateProductCommand>,
            CreateProductHandler>();
        services.AddScoped<
            ICommandHandler<Guid, UploadProductsPhotosCommand>,
            UploadProductsPhotosHandler>();
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

        return services;
    }
}