namespace Products.Contracts;

public record UploadProductsPhotosRequestDto(Guid ProductId, UploadFileDto[] FileDtos);