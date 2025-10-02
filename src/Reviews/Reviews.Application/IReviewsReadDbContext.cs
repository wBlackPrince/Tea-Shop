using Reviews.Domain;

namespace Reviews.Application;

public interface IReviewsReadDbContext
{
    IQueryable<Review> ReviewsRead { get; }
}