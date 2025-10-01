namespace Users.Contracts;

public record GetUserWithPaginationRequestDto(
    Guid UserId,
    DateTime? DateFrom,
    DateTime? DateTo,
    PaginationRequest Pagination);