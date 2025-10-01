using FluentValidation;
using Orders.Contracts;
using Orders.Domain;

namespace Orders.Application.Commands.CreateOrderCommand;

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
}