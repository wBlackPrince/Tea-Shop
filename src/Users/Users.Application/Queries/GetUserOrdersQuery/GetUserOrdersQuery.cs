using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUserOrdersQuery;

public record GetUserOrdersQuery(GetUserOrdersRequestDto Request): IQuery;