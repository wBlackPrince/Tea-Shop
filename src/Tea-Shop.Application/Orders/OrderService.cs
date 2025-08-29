using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Orders;

public class OrderService: IOrdersService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrdersRepository _productsRepository;
    private readonly IValidator<CreateOrderRequestDto> _createOrderValidator;

    public OrderService(
        ILogger<OrderService> logger,
        IOrdersRepository productsRepository,
        IValidator<CreateOrderRequestDto> createOrderValidator)
    {
        _logger = logger;
        _productsRepository = productsRepository;
        _createOrderValidator = createOrderValidator;
    }

    public async Task<Guid> CreateOrder(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var orderItems = request.Items.Select(i =>
                OrderItem.Create(
                    new OrderItemId(Guid.NewGuid()),
                    new ProductId(i.ProductId),
                    i.Quantity).Value)
            .ToList();

        var order = new Order(
            new OrderId(Guid.NewGuid()),
            new UserId(request.UserId),
            request.DeliveryAddress,
            (PaymentWay)Enum.Parse(typeof(PaymentWay), request.PaymentMethod),
            request.ExpectedTimeDelivery,
            (OrderStatus)Enum.Parse(typeof(OrderStatus), request.Status),
            orderItems,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _productsRepository.CreateOrder(order, cancellationToken);

        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Create order {orderId}", order.Id);

        return order.Id.Value;
    }
}