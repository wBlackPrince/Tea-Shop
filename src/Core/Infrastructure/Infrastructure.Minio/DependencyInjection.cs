using AppFileProvider = Tea_Shop.Application.FilesStorage.IFileProvider;

namespace Minio;

public static class DependencyInjection
{
    public static IServiceCollection AddMinioDependencies(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<MinioOptions>(config.GetSection("Infrastructure.Minio"));

        services.AddSingleton(sp =>
        {
            var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MinioOptions>>().Value;
            var client = new Minio.MinioClient()
                .WithEndpoint(opt.Endpoint)
                .WithCredentials(opt.AccessKey, opt.SecretKey)
                .Build();

            if (opt.UseSsl) client = client.WithSSL();  // <-- только если надо!

            return client;
        });

        services.AddScoped<AppFileProvider, MinioProvider>();

        return services;
    }
}