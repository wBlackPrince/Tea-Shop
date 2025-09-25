using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.UploadProductsPhotosCommand;

public record UploadProductsPhotosCommand(UploadProductsPhotosRequestDto Request): ICommand;

