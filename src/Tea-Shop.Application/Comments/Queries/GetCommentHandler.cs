using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Application.Comments.Queries;

public class GetCommentByIdHandler
{
    private readonly IReadDbContext _readDbContext;

    public GetCommentByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Guid> Handle(Guid commentId, CancellationToken cancellationToken)
    {
        await _readDbContext.CommentsRead.FirstOrDefaultAsync(
            c => c.Id == new CommentId(commentId),
            cancellationToken);

        return commentId;
    }
}