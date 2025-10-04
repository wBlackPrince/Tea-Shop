using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.FilesStorage;

namespace Minio;

public static class DependencyInjection
{
    public static IServiceCollection AddMinioDependencies(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<MinioOptions>(config.GetSection("Minio"));

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

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }
}