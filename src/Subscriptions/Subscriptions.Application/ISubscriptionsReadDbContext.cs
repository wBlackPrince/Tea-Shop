using Subscriptions.Domain;

namespace Subscriptions.Application;

public interface ISubscriptionsReadDbContext
{
    public IQueryable<Kit> KitsRead { get; }
}