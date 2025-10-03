namespace Users.Contracts.Dtos;

public record GetUserResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Role,
    int BonusPoints,
    Guid BasketId,
    Guid? AvatarId,
    string MiddleName);