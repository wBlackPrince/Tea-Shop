using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands;

public class CreateOrderHandler
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IValidator<CreateOrderRequestDto> _validator;

    public CreateOrderHandler(
        IOrdersRepository ordersRepository,
        IReadDbContext readDbContext,
        ILogger<CreateOrderHandler> logger,
        IValidator<CreateOrderRequestDto> validator)
    {
        _ordersRepository = ordersRepository;
        _readDbContext = readDbContext;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        // валидация входных параметров
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // валидация бизнес-логики
        // проверка того что все товары заказа доступны для покупки
        Product? product = null;

        for (int i = 0; i < request.Items.Length; i++)
        {
            product = await _readDbContext.ProductsRead
                .FirstOrDefaultAsync(
                    p => p.Id == new ProductId(request.Items[i].ProductId),
                    cancellationToken);

            if (product is null)
            {
                return Error.NotFound("create.order", "product not found");
            }

            if (product.StockQuantity < request.Items[i].Quantity)
            {
                return Error.Failure("create.order", "There is not enough stock to order item");
            }
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
}