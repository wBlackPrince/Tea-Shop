using Tea_Shop.Contract.Users;

namespace Tea_Shop.Contract.Orders;

public record GetUserOrdersResponse(
    GetUserWithExtraInfoAboutOrdersDto UserInfo,
    OrderDto[] Orders);