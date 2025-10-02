namespace Users.Contracts.Dtos;

public record GetUsersResponseDto(GetUserResponseDto[] Users, long TotalCount);