namespace Tea_Shop.Domain.Orders;

/// <summary>
/// Способ оплаты заказа
/// </summary>
public enum PaymentWay
{
    /// <summary> Наличными при получении </summary>
    CashOnDelivery,

    /// <summary> Банковской картой онлайн </summary>
    CardOnline,

    /// <summary> Картой при получении (POS-терминал) </summary>
    CardOnDelivery,

    /// <summary> Переводом по реквизитам (СБП, IBAN) </summary>
    BankTransfer
}