namespace Subscriptions.Domain;

public class ActiveState : SubscriptionState
{
    public ActiveState(
        SubscriptionStatus status,
        int statusDuration)
    {
        Status = status;
        StatusDuration = statusDuration;
    }

    public SubscriptionStatus Status { get; set; }

    public int StatusDuration { get; set; }
}