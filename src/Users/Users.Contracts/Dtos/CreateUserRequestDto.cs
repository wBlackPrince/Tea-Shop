using Shared.Dto;

namespace Users.Contracts.Dtos;

public record CreateUserRequestDto(
    string Password,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Role,
    string MiddleName,
    UploadFileDto? FileDto);