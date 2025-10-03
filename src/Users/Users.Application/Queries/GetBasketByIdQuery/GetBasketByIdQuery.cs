using Shared.Abstractions;
using Shared.ValueObjects;

namespace Users.Application.Queries.GetBasketByIdQuery;

public record GetBasketByIdQuery(BasketId BasketId): IQuery;