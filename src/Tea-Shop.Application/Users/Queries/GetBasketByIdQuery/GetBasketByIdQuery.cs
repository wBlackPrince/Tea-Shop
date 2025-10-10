using Tea_Shop.Application.Abstractions;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Users.Queries.GetBasketByIdQuery;

public record GetBasketByIdQuery(BasketId BasketId): IQuery;