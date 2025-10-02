using Shared.Dto;

namespace Users.Contracts.Dtos;

public record GetUserWithPaginationRequestDto(
    Guid UserId,
    DateTime? DateFrom,
    DateTime? DateTo,
    PaginationRequest Pagination);