using Microsoft.Extensions.DependencyInjection;

namespace Tea_Shop.Infrastructure.Postgres.Seeders;

public static class SeederExtensions
{
    public static async Task<IServiceProvider> RunSeeding(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetService<ISeeder>();
        await seeder.SeedAsync();

        return services;
    }
}