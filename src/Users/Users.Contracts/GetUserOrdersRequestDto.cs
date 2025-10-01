namespace Users.Contracts;

public record GetUserOrdersRequestDto(
    UserWithOnlyIdDto UserDto,
    PaginationRequest Pagination);