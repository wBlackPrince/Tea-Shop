using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users.Queries.GetUserByIdQuery;

public class GetUserByIdHandler:
    IQueryHandler<GetUserResponseDto?, GetUserByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetUserByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<GetUserResponseDto?> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        User? user = await _readDbContext.UsersRead.FirstOrDefaultAsync(
            u => u.Id == new UserId(query.Request.UserId),
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        var response = new GetUserResponseDto(
            user.Id.Value,
            user.FirstName,
            user.LastName,
            user.Role.ToString(),
            user.AvatarId,
            user.MiddleName);

        return response;
    }
}