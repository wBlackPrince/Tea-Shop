using Tea_Shop.Contract.Products;

namespace Tea_Shop.Contract.Users;

public record GetUserCommentsRequestDto(
    UserWithOnlyIdDto UserDto,
    PaginationRequest Pagination);