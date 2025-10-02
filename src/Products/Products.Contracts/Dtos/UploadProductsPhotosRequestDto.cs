using Shared.Dto;

namespace Products.Contracts.Dtos;

public record UploadProductsPhotosRequestDto(Guid ProductId, UploadFileDto[] FileDtos);