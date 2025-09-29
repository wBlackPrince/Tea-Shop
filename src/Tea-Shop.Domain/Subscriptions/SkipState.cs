namespace Tea_Shop.Domain.Subscriptions;

public class SkipState : SubscriptionState
{
    public SkipState(
        SubscriptionStatus status,
        int statusDuration)
    {
        Status = status;
        StatusDuration = statusDuration;
    }

    public SubscriptionStatus Status { get; set; }

    public int StatusDuration { get; set; }
}