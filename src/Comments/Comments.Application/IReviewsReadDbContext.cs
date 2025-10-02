using Comments.Domain;

namespace Comments.Application;

public interface IReviewsReadDbContext
{
    IQueryable<Review> ReviewsRead { get; }
}