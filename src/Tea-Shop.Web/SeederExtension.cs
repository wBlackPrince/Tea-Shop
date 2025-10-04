namespace Tea_Shop.Web;

public static class SeederExtension
{
    public static async Task<IServiceProvider> RunSeeding(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var seeder = scope.ServiceProvider.GetService<Seeders>();

        await seeder?.SeedAsync();


        return services;
    }
}