namespace Shared.Dto;

public record UploadProductPhotosHttpRequestDto
{
    public IEnumerable<IFormFile> ProductsFiles { get; init; }
}