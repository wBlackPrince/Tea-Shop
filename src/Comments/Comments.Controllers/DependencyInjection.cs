using Comments.Application;
using Commnets.Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.Controllers;

public static class DependencyInjection
{
    public static IServiceCollection AddCommentsDependencies(this IServiceCollection services)
    {
        services.AddPostgresDependencies();
        services.AddCommentsApplication();

        return services;
    }
}