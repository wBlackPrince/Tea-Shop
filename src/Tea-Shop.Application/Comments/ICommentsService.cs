using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public interface ICommentsService
{
    Task<Guid> CreateComment(
        CreateCommentRequestDto request,
        CancellationToken cancellationToken);
}