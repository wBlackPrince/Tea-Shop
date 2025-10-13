using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Subscriptions;

namespace Tea_Shop.Application.Subscriptions.Queries.GetKitByIdQuery;

public record GetKitByIdQuery(KitWithOnlyIdDto Request): IQuery;