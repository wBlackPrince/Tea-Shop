using Shared.Dto;

namespace Users.Contracts.Dtos;

public record GetUserOrdersRequestDto(
    UserWithOnlyIdDto UserDto,
    PaginationRequest Pagination);