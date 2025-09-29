namespace Tea_Shop.Domain.Subscriptions;

public class PausedState: SubscriptionState
{
    public PausedState(SubscriptionStatus status)
    {
        Status = status;
    }

    public SubscriptionStatus Status { get; set; }
}