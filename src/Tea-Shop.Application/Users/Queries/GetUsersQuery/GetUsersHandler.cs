using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Users.Queries.GetUsersQuery;

public class GetUsersHandler(
    IReadDbContext readDbContext,
    ILogger<GetUsersHandler> logger):
    IQueryHandler<GetUsersResponseDto, GetUsersQuery>
{
    public async Task<GetUsersResponseDto> Handle(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetUsersHandler));

        var usersQuery = readDbContext.UsersRead;

        if (!string.IsNullOrWhiteSpace(query.Request.SearchFirstName))
        {
            usersQuery = usersQuery.Where(u => EF.Functions.Like(
                u.FirstName.ToLower(),
                $"%{query.Request.SearchFirstName.ToLower()}%"));
        }

        if (!string.IsNullOrWhiteSpace(query.Request.SearchLastName))
        {
            usersQuery = usersQuery.Where(u => EF.Functions.Like(
                u.LastName.ToLower(),
                $"%{query.Request.SearchLastName.ToLower()}%"));
        }

        if (!string.IsNullOrWhiteSpace(query.Request.SearchMiddleName))
        {
            usersQuery = usersQuery.Where(u => EF.Functions.Like(
                u.MiddleName.ToLower(),
                $"%{query.Request.SearchMiddleName.ToLower()}%"));
        }

        if (!string.IsNullOrWhiteSpace(query.Request.Role))
        {
            usersQuery = usersQuery.Where(u => u.Role == query.Request.Role);
        }

        if (query.Request.IsActive.HasValue)
        {
            usersQuery = usersQuery.Where(u => u.IsActive == query.Request.IsActive);
        }

        int totalCount = usersQuery.Count();

        usersQuery = usersQuery
            .OrderBy(u => u.LastName)
            .Skip(
                (query.Request.Pagination.Page - 1) *
                query.Request.Pagination.PageSize)
            .Take(query.Request.Pagination.PageSize);

        var users = await usersQuery
            .Select(u => new GetUserResponseDto(
                u.Id.Value,
                u.FirstName,
                u.LastName,
                u.Role.ToString(),
                u.BasketId.Value,
                u.AvatarId,
                u.MiddleName))
            .ToArrayAsync(cancellationToken);

        if (users.Length == 0)
        {
            logger.LogWarning("Users with these filters not found.");
        }

        logger.LogDebug("Users with these filters found.");

        return new GetUsersResponseDto(users, totalCount);
    }
}