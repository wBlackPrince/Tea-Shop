using Comments.Domain;

namespace Comments.Application;

public interface ICommentsReadDbContext
{
    IQueryable<Comment> CommentsRead { get; }
    
    IQueryable<Review> ReviewsRead { get; }
}