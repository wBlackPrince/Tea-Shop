namespace Tea_Shop.Contract.Users;

public record GetUsersResponseDto(GetUserResponseDto[] Users, long TotalCount);