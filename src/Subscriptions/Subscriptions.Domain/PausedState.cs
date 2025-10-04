namespace Subscriptions.Domain;

public class PausedState: SubscriptionState
{
    public PausedState(SubscriptionStatus status)
    {
        Status = status;
    }

    public SubscriptionStatus Status { get; set; }
}