namespace Users.Contracts;

public record GetUsersResponseDto(GetUserResponseDto[] Users, long TotalCount);