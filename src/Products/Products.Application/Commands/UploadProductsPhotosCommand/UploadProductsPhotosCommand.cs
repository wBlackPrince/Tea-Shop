using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Commands.UploadProductsPhotosCommand;

public record UploadProductsPhotosCommand(UploadProductsPhotosRequestDto Request): ICommand;

