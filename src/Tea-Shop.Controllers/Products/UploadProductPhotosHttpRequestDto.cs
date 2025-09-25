using Microsoft.AspNetCore.Http;

namespace Tea_Shop.Products;

public record UploadProductPhotosHttpRequestDto
{
    public IEnumerable<IFormFile> ProductsFiles { get; init; }
}