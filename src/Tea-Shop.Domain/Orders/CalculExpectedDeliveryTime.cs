using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Orders;

public static class CalculExpectedDeliveryTime
{
    public static DateTime Calcul(
        DateTime orderDate,
        string deliveryAddress,
        DeliveryWay deliveryWay)
    {
        DateTime deliveryTime = orderDate;

        // учитываем выходны дни
        if (orderDate.DayOfWeek == DayOfWeek.Saturday)
        {
            deliveryTime = deliveryTime.AddDays(2);
        }
        else if (orderDate.DayOfWeek == DayOfWeek.Friday && orderDate.ToLocalTime().Hour > Constants.EndOfWorkingDay)
        {
            deliveryTime = deliveryTime.AddHours(56);
        }
        else if (orderDate.DayOfWeek == DayOfWeek.Sunday || orderDate.Hour > Constants.EndOfWorkingDay)
        {
            deliveryTime = deliveryTime.AddDays(1);
        }

        if (deliveryWay == DeliveryWay.Courier)
        {
            deliveryTime = deliveryTime.AddHours(12);
        }



        // проверяем что заказ нужно доставить в соседний город или село
        if (!deliveryAddress.Contains("Cheboksary"))
        {
            deliveryTime = deliveryTime.AddDays(1);
        }

        return deliveryTime;
    }
}