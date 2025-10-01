using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Queries.GetUserOrdersQuery;

public record GetUserOrdersQuery(GetUserOrdersRequestDto Request): IQuery;