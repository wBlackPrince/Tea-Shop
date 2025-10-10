using Tea_Shop.Application.Abstractions;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Users.Queries.GetBasketItemByIdQuery;

public record GetBasketItemByIdQuery(BasketItemId BasketId): IQuery;