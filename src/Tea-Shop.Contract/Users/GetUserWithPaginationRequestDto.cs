using Tea_Shop.Contract.Products;

namespace Tea_Shop.Contract.Users;

public record GetUserWithPaginationRequestDto(
    Guid UserId,
    PaginationRequest Pagination);