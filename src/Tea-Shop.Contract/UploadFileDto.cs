namespace Tea_Shop.Contract;

public sealed record UploadFileDto(
    Stream Stream,
    string FileName,
    string? ContentType = null
);