namespace Subscriptions.Domain;

public class SubscriptionState
{
    private const string delimiter = "-";

    public static string ConverToString(SubscriptionState state)
    {
        string strState = string.Empty;

        if (state is ActiveState || state is SkipState)
        {
            ActiveState activeState = state as ActiveState;
            strState = activeState.Status.ToString();
            strState += delimiter;
            strState += activeState.StatusDuration.ToString();
        }
        else
        {
            PausedState pauseState = state as PausedState;
            strState = pauseState.Status.ToString();
        }

        return strState;
    }

    public static SubscriptionState ConverToState(string strState)
    {
        string[] strAttributes = strState.Split(delimiter);

        if (strAttributes.Length > 1)
        {
            if (strAttributes[0] == "MONTHLY")
            {
                return new ActiveState(
                    SubscriptionStatus.MONTHLY,
                    int.Parse(strAttributes[1]));
            }
            else
            {
                return new SkipState(
                    SubscriptionStatus.SKIP,
                    int.Parse(strAttributes[1]));
            }
        }
        else
        {
            return new PausedState(SubscriptionStatus.PAUSE);
        }
    }
}