using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Baskets;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.CreateOrderCommand;

public class CreateOrderHandler: ICommandHandler<CreateOrderResponseDto, CreateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IBasketsRepository _basketsRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IValidator<CreateOrderRequestDto> _validator;
    private readonly ITransactionManager _transactionManager;

    public CreateOrderHandler(
        IOrdersRepository ordersRepository,
        IUsersRepository usersRepository,
        IBasketsRepository basketsRepository,
        IReadDbContext readDbContext,
        ILogger<CreateOrderHandler> logger,
        IValidator<CreateOrderRequestDto> validator,
        ITransactionManager transactionManager)
    {
        _ordersRepository = ordersRepository;
        _usersRepository = usersRepository;
        _basketsRepository = basketsRepository;
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
        BasketItem? basketItem = null;
        BasketItemId? basketItemId = null;
        User? user = await _usersRepository.GetUserById(
            new UserId(command.Request.UserId),
            cancellationToken);
        Result<OrderItem, Error> orderItem;
        Basket? basket = await _basketsRepository.GetById(
            user?.BasketId,
            cancellationToken);

        List<OrderItem> orderItems = new List<OrderItem>();

        for (int i = 0; i < command.Request.Items.Length; i++)
        {
            basketItemId = new BasketItemId(command.Request.Items[i].BasketItemId);

            basketItem = await _readDbContext.BusketsItemsRead
                .FirstOrDefaultAsync(bi => bi.Id == basketItemId, cancellationToken);


            // если такого продукта не существует
            if (product is null)
            {
                _logger.LogError("No product with id {productId} found", product?.Id.Value);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "product not found");
            }

            // если такого товара в корзине не существует
            if (basketItem is null)
            {
                _logger.LogError("No basket item with id {basketItemId} found", basketItemId?.Value);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "basket item not found");
            }


            product = await _readDbContext.ProductsRead
                .FirstOrDefaultAsync(
                    p => p.Id == basketItem.ProductId,
                    cancellationToken);

            // если число товаров в корзине меньше чем в запросе
            if (basketItem.Quantity < command.Request.Items[i].Quantity)
            {
                _logger.LogError("There are too low count of basket items");

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "There are too low count of basket items");
            }

            // если число товаров на складе меньше чем в запросе
            if (product?.StockQuantity < command.Request.Items[i].Quantity)
            {
                _logger.LogError(
                    "Product with id {productId} doesn't have enough stock quantity",
                    product?.Id.Value);

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "There is not enough stock to order item");
            }

            // Если превышано ограничение на число товаров
            if (command.Request.Items[i].Quantity > 15 && command.Request.PaymentMethod == "CashOnDelivery")
            {
                _logger.LogError("Order with more than 15 items and with pay after delivery cannot be created");

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "You cannot order 15 or more items and pay after delivery");
            }


            product.StockQuantity -= command.Request.Items[i].Quantity;

            orderItem = OrderItem.Create(
                new OrderItemId(Guid.NewGuid()),
                product.Id,
                command.Request.Items[i].Quantity);

            if (orderItem.IsSuccess)
            {
                orderItems.Add(orderItem.Value);
                basketItem.Quantity -= command.Request.Items[i].Quantity;
                if (basketItem.Quantity <= 0)
                {
                    basket?.RemoveItem(basketItem);
                }
            }
            else
            {
                return orderItem.Error;
            }
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