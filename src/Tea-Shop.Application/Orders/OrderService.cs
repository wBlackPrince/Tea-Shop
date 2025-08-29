using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders;

public class OrderService: IOrdersService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<CreateOrderRequestDto> _createOrderValidator;

    public OrderService(
        ILogger<OrderService> logger,
        IOrdersRepository ordersRepository,
        IValidator<CreateOrderRequestDto> createOrderValidator)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
        _createOrderValidator = createOrderValidator;
    }


    public async Task<Result<GetOrderResponseDto, Error>> GetOrderById(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, order, error) = await _ordersRepository.GetOrderById(
            new OrderId(orderId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        OrderItemDto[] order_items = order.OrderItems
            .Select(oi => new OrderItemDto(oi.ProductId.Value, oi.Quantity))
            .ToArray();

        GetOrderResponseDto response = new GetOrderResponseDto(
            order.Id.Value,
            order.UserId.Value,
            order.DeliveryAddress,
            order.PaymentWay.ToString(),
            order.ExpectedDeliveryTime,
            order.OrderStatus.ToString(),
            order_items,
            order.CreatedAt,
            order.CreatedAt);

        return response;
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

        await _ordersRepository.CreateOrder(order, cancellationToken);

        await _ordersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Create order {orderId}", order.Id);

        return order.Id.Value;
    }


    public async Task<Result<Guid, Error>> DeleteOrder(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _ordersRepository
            .DeleteOrder(new OrderId(orderId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error;
        }

        _logger.LogInformation("Deleted order {orderId}", orderId);

        return orderId;
    }
}