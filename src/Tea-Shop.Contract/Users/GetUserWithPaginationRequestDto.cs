using Tea_Shop.Contract.Products;

namespace Tea_Shop.Contract.Users;

public record GetUserWithPaginationRequestDto(
    Guid UserId,
    DateTime? DateFrom,
    DateTime? DateTo,
    PaginationRequest Pagination);