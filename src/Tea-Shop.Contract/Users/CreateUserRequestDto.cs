namespace Tea_Shop.Contract.Users;

public record CreateUserRequestDto(
    string Password,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Role,
    Guid? AvatarId,
    string MiddleName,
    UploadFileDto? FileDto);