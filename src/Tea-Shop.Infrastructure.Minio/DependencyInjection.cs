using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Tea_Shop.Application.FilesStorage;

namespace Tea_Shop.Infrastructure.S3;

public static class DependencyInjection
{
    // public static IServiceCollection AddMinioDependencies(
    //     this IServiceCollection services,
    //     IConfiguration config)
    // {
    //     services.AddSingleton(sp =>
    //     {
    //         services.Configure<MinioOptions>(config.GetSection("Minio"));
    //
    //         var opt = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MinioOptions>>().Value;
    //         var client = new Minio.MinioClient()
    //             .WithEndpoint(opt.Endpoint)
    //             .WithCredentials(opt.AccessKey, opt.SecretKey)
    //             .WithSSL()
    //             .Build();
    //
    //         return client;
    //     });
    //
    //     services.AddSingleton<IFileProvider, MinioProvider>();
    //
    //     return services;
    // }
}