namespace Tea_Shop.Contract.Products;

public record UploadProductsPhotosRequestDto(Guid ProductId, UploadFileDto[] FileDtos);