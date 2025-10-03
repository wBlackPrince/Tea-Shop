using Comments.Application;
using Commnets.Infrastructure.Postgres.Database;
using Commnets.Infrastructure.Postgres.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Database;

namespace Commnets.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresDependencies(this IServiceCollection services)
    {
        services.AddScoped<ICommentsRepository, CommentsRepository>();

        services.AddSingleton<ISqlConnectionFactory, CommentsSqlConnectionFactory>();
        services.AddSingleton<IDbConnectionFactory, CommentsNpgSqlConnectionFactory>();
        services.AddScoped<ITransactionManager, CommentsTransactionManager>();

        return services;
    }
}