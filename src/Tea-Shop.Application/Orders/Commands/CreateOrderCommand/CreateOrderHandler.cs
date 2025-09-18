using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.CreateOrderCommand;

public class CreateOrderHandler: ICommandHandler<CreateOrderResponseDto, CreateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IValidator<CreateOrderRequestDto> _validator;
    private readonly ITransactionManager _transactionManager;

    public CreateOrderHandler(
        IOrdersRepository ordersRepository,
        IReadDbContext readDbContext,
        ILogger<CreateOrderHandler> logger,
        IValidator<CreateOrderRequestDto> validator,
        ITransactionManager transactionManager)
    {
        _ordersRepository = ordersRepository;
        _readDbContext = readDbContext;
        _logger = logger;
        _validator = validator;
        _transactionManager = transactionManager;
    }

    public async Task<Result<CreateOrderResponseDto, Error>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handlerName}", nameof(CreateOrderHandler));

        // валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError(
                "Validation failed {validationError}",
                validationResult.Errors.First().ErrorMessage);
            return Error.Validation(
                "create.order",
                $"Validation failed {validationResult.Errors.First().ErrorMessage}");
        }

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating order");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        // валидация бизнес-логики
        // проверка того что все товары заказа доступны для покупки
        Product? product = null;
        ProductId? productId = null;

        for (int i = 0; i < command.Request.Items.Length; i++)
        {
            productId = new ProductId(command.Request.Items[i].ProductId);
            product = await _readDbContext.ProductsRead
                .FirstOrDefaultAsync(
                    p => p.Id == productId,
                    cancellationToken);

            if (product is null)
            {
                _logger.LogError("No product with id {productId} found", productId.Value);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "product not found");
            }

            if (product.StockQuantity < command.Request.Items[i].Quantity)
            {
                _logger.LogError(
                    "Product with id {productId} doesn't have enough stock quantity",
                    productId.Value);

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "There is not enough stock to order item");
            }

            if (command.Request.Items[i].Quantity > 15 && command.Request.PaymentMethod == "CashOnDelivery")
            {
                _logger.LogError("Order with more than 15 items and with pay after delivery cannot be created");

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "You cannot order 15 or more items and pay after delivery");
            }

            product.StockQuantity -= command.Request.Items[i].Quantity;
        }

        // проверка ограничения числа товаров в заказа
        if (command.Request.Items.Length > 20)
        {
            _logger.LogError("Too many order items (more than 20 items)");

            transactionScope.Rollback();

            return Error.Failure(
                "create.order",
                "Too many order items");
        }

        var orderItems = command.Request.Items.Select(i =>
                OrderItem.Create(
                    new OrderItemId(Guid.NewGuid()),
                    new ProductId(i.ProductId),
                    i.Quantity).Value)
            .ToList();

        var order = new Order(
            new OrderId(Guid.NewGuid()),
            new UserId(command.Request.UserId),
            command.Request.DeliveryAddress,
            (PaymentWay)Enum.Parse(typeof(PaymentWay), command.Request.PaymentMethod),
            command.Request.ExpectedTimeDelivery,
            orderItems,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _ordersRepository.CreateOrder(order, cancellationToken);


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while creating order");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Create order {orderId}", order.Id);


        var response = new CreateOrderResponseDto
        {
            Id = order.Id.Value,
            UserId = order.UserId.Value,
            DeliveryAddress = order.DeliveryAddress,
            PaymentMethod = order.PaymentWay.ToString(),
            Status = order.OrderStatus.ToString(),
            ExpectedTimeDelivery = order.ExpectedDeliveryTime,
            Items = order.OrderItems
                .Select(o =>
                    new OrderItemDto(o.ProductId.Value, o.Quantity))
                .ToArray(),
        };

        return response;
    }
}