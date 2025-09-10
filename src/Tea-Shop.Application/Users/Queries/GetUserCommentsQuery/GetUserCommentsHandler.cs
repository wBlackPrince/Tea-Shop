using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;

public class GetUserCommentsHandler:
    IQueryHandler<GetUserCommentsResponse, GetUserCommentsQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetUserCommentsHandler> _logger;

    public GetUserCommentsHandler(
        IDbConnectionFactory dbConnectionFactory,
        ILogger<GetUserCommentsHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<GetUserCommentsResponse> Handle(
        GetUserCommentsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        throw new NotImplementedException();
    }
}