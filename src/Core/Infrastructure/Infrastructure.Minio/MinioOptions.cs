namespace Minio;

public sealed class MinioOptions
{
    public string Endpoint { get; init; } = default!;

    public string AccessKey { get; init; } = default!;

    public string SecretKey { get; init; } = default!;

    public bool UseSsl { get; init; } = false;

    public string? PublicBaseUrl { get; init; }
}