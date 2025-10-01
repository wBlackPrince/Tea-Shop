namespace Users.Contracts;

public record GetUserResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Role,
    Guid BasketId,
    Guid? AvatarId,
    string MiddleName);