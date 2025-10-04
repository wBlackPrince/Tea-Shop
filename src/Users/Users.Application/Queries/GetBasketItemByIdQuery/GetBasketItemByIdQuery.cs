using Shared.Abstractions;
using Shared.ValueObjects;

namespace Users.Application.Queries.GetBasketItemByIdQuery;

public record GetBasketItemByIdQuery(BasketItemId BasketId): IQuery;