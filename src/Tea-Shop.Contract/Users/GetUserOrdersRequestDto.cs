using Tea_Shop.Contract.Products;

namespace Tea_Shop.Contract.Users;

public record GetUserOrdersRequestDto(
    UserWithOnlyIdDto UserDto,
    PaginationRequest Pagination);