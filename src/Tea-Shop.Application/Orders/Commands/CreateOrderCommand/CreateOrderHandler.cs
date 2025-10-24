using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Orders.Commands.CreateOrderCommand;

public class CreateOrderHandler(
    IOrdersRepository ordersRepository,
    IUsersRepository usersRepository,
    IReadDbContext readDbContext,
    ILogger<CreateOrderHandler> logger,
    IValidator<CreateOrderRequestDto> validator,
    ITransactionManager transactionManager): ICommandHandler<CreateOrderResponseDto, CreateOrderCommand>
{
    public async Task<Result<CreateOrderResponseDto, Error>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handlerName}", nameof(CreateOrderHandler));

        // валидация входных параметров
        var validationResult = await validator.ValidateAsync(command.Request, cancellationToken);
        if (!validationResult.IsValid)
        {
            logger.LogError(
                "Validation failed {validationError}",
                validationResult.Errors.First().ErrorMessage);
            return Error.Validation(
                "order.create",
                $"Validation failed {validationResult.Errors.First().ErrorMessage}",
                validationResult.Errors.First().PropertyName);
        }


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating order");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        // валидация бизнес-логики
        // проверка того что все товары заказа доступны для покупки
        Product? product = null;
        BasketItem? basketItem = null;
        BasketItemId? basketItemId = null;
        User? user = await usersRepository.GetUserById(
            new UserId(command.Request.UserId),
            cancellationToken);
        Result<OrderItem, Error> orderItem;
        Basket? basket = await usersRepository.GetBasketById(
            user?.BasketId,
            cancellationToken);

        // если такой корзины не существует
        if (basket is null)
        {
            logger.LogError("No basket with id {basketId} found", basket?.Id.Value);
            transactionScope.Rollback();
            return Error.NotFound(
                "create.order",
                "basket not found");
        }

        List<OrderItem> orderItems = new List<OrderItem>();
        float orderSum = 0;
        int newBonuses = 0;

        for (int i = 0; i < command.Request.Items.Length; i++)
        {
            basketItemId = new BasketItemId(command.Request.Items[i].BasketItemId);

            basketItem = await usersRepository.GetBasketItemById(basketItemId, cancellationToken);

            // если такой вещи корзины не существует
            if (basketItem is null)
            {
                logger.LogError("No basket item with id {basketItemId} found", basketItem?.Id.Value);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "basket item not found");
            }

            product = await readDbContext.ProductsRead.FirstOrDefaultAsync(
                p => p.Id == basketItem.ProductId,
                cancellationToken);

            // если такого продукта не существует
            if (product is null)
            {
                logger.LogError("No product with id {productId} found", product?.Id.Value);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "product not found");
            }


            product = await readDbContext.ProductsRead
                .FirstOrDefaultAsync(
                    p => p.Id == basketItem.ProductId,
                    cancellationToken);

            // если число товаров в корзине меньше чем в запросе
            if (basketItem.Quantity < command.Request.Items[i].Quantity)
            {
                logger.LogError("There are too low count of basket items");

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "There are too low count of basket items");
            }

            // если число товаров на складе меньше чем в запросе
            if (product?.StockQuantity < command.Request.Items[i].Quantity)
            {
                logger.LogError(
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
                logger.LogError("Order with more than 15 items and with pay after delivery cannot be created");

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    "You cannot order 15 or more items and pay after delivery");
            }

            newBonuses += (int)(product.Price * 0.15f * command.Request.Items[i].Quantity);
            orderSum += product.Price;

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
            logger.LogError("Too many order items (more than 20 items)");

            transactionScope.Rollback();

            return Error.Failure(
                "create.order",
                "Too many order items");
        }

        PaymentWay paymentWay = (PaymentWay)Enum.Parse(typeof(PaymentWay), command.Request.PaymentMethod);
        DeliveryWay deliveryWay = (DeliveryWay)Enum.Parse(typeof(DeliveryWay), command.Request.DeliveryWay);

        DateTime createAt = DateTime.UtcNow;
        DateTime updatedAt = DateTime.UtcNow;
        DateTime expectedDeliveryDate = CalculExpectedDeliveryTime.Calcul(
            createAt,
            command.Request.DeliveryAddress,
            deliveryWay);

        var order = new Order(
            new OrderId(Guid.NewGuid()),
            new UserId(command.Request.UserId),
            command.Request.DeliveryAddress,
            paymentWay,
            deliveryWay,
            expectedDeliveryDate,
            orderItems,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await ordersRepository.CreateOrder(order, cancellationToken);


        if (command.Request.UsedBonuses > orderSum)
        {
            orderSum = 0;
        }
        else
        {
            orderSum = orderSum - command.Request.UsedBonuses;
        }

        user.RemoveBonusPoints(command.Request.UsedBonuses);

        user.AddBonusPoints(newBonuses);

        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating order");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Create order {orderId}", order.Id);


        var response = new CreateOrderResponseDto
        {
            Id = order.Id.Value,
            UserId = order.UserId.Value,
            DeliveryAddress = order.DeliveryAddress,
            PaymentMethod = order.PaymentWay.ToString(),
            Status = order.OrderStatus.ToString(),
            ExpectedTimeDelivery = order.ExpectedDeliveryTime,
            OrderSum = orderSum,
            Items = order.OrderItems
                .Select(o =>
                    new OrderItemResponseDto{
                        ProductId = o.ProductId.Value,
                        Quantity = o.Quantity,
                    })
                .ToArray(),
        };

        return response;
    }
}