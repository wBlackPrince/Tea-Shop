using Comments.Application.Commands.CreateCommentCommand;
using Comments.Application.Commands.CreateReviewCommand;
using Comments.Application.Commands.DeleteCommentCommand;
using Comments.Application.Commands.DeleteReviewCommand;
using Comments.Application.Commands.UpdateCommentCommand;
using Comments.Application.Commands.UpdateReviewCommand;
using Comments.Application.Queries.GetCommentByIdQuery;
using Comments.Application.Queries.GetCommentChildCommentsQuery;
using Comments.Application.Queries.GetReviewByIdQuery;
using Comments.Application.Queries.GetReviewCommentsQuery;
using Comments.Contracts.Dtos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions;

namespace Comments.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCommentsApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

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

        return services;
    }
}