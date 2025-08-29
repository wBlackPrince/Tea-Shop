namespace Tea_Shop.Contract.Users;

public record GetUserResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Role,
    Guid? AvatarId,
    string MiddleName);