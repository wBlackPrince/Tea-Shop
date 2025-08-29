using System.Globalization;
using FluentValidation;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application.Products;

public class CreateOrderValidator: AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderValidator()
    {
        this
            .RuleFor(o => o.DeliveryAddress)
            .NotEmpty().WithMessage("Delivery address is required")
            .NotNull().WithMessage("Delivery address is required");

        this
            .RuleFor(o => o.PaymentMethod)
            .Must(BeValidPaymentWay).WithMessage("Payment method is invalid");

        this
            .RuleFor(o => o.Status)
            .Must(BeValidOrderStatus).WithMessage("Order status is invalid");

        this
            .RuleForEach(o => o.Items)
            .ChildRules(items =>
            {
                items
                    .RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity of order item must be greater than zero");
            });
    }

    private bool BeValidPaymentWay(string paymentWay)
    {
        return PaymentWay.TryParse(typeof(PaymentWay), paymentWay, out _);
    }

    private bool BeValidOrderStatus(string orderStatus)
    {
        return OrderStatus.TryParse(typeof(OrderStatus), orderStatus, out _);
    }
}