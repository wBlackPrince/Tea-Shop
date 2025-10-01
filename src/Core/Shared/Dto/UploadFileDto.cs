namespace Shared.Dto;

public sealed record UploadFileDto(
    Stream Stream,
    string FileName,
    string? ContentType = null
);