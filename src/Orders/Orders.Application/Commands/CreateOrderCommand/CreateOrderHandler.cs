using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Orders.Contracts;
using Orders.Contracts.Dtos;
using Orders.Domain;
using Products.Contracts;
using Products.Contracts.Dtos;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.Dto;
using Shared.ValueObjects;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Orders.Application.Commands.CreateOrderCommand;

public class CreateOrderHandler(
    IOrdersRepository ordersRepository,
    IUsersContracts usersContracts,
    IProductsContracts productsContracts,
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
                "create.order",
                $"Validation failed {validationResult.Errors.First().ErrorMessage}");
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
        BasketItemId? basketItemId = null;
        var user = await usersContracts.GetUserById(
            new UserWithOnlyIdDto(command.Request.UserId),
            cancellationToken);

        if (user is null)
        {
            logger.LogError("No user with id {basketId} found", user?.Id);
            transactionScope.Rollback();
            return Error.NotFound(
                "create.order",
                "user not found");
        }

        Result<OrderItem, Error> orderItem;
        var basket = await usersContracts.GetBasketById(
            new BasketId(user.BasketId),
            cancellationToken);

        // если такой корзины не существует
        if (basket is null)
        {
            logger.LogError("No basket with id {basketId} found", basket?.Id);
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

            var basketItem = await usersContracts.GetBasketItemById(basketItemId, cancellationToken);

            // если такой вещи корзины не существует
            if (basketItem is null)
            {
                logger.LogError("No basket item with id {basketItemId} found", basketItem?.Id);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "basket item not found");
            }

            var product = await productsContracts.GetProductById(
                new ProductWithOnlyIdDto(basketItem.ProductId),
                cancellationToken);

            // если такого продукта не существует
            if (product is null)
            {
                logger.LogError("No product with id {productId} found", product?.Id);
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.order",
                    "product not found");
            }

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
                    product?.Id);

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


            //product.StockQuantity -= command.Request.Items[i].Quantity;
            var updateResult = await productsContracts.UpdateProduct(
                product.Id,
                new UpdateEntityRequestDto(
                    "StockQuantity", 
                    product?.StockQuantity - command.Request.Items[i].Quantity),
                cancellationToken);

            if (updateResult.IsFailure)
            {
                logger.LogError(
                    "Cannot update product's stock quantity with {productId}",
                    product?.Id);

                transactionScope.Rollback();

                return Error.Failure(
                    "create.order",
                    $"Cannot update product's stock quantity: {updateResult.Error.Message}");
            }

            newBonuses += (int)(product.Price * 0.15f * command.Request.Items[i].Quantity);
            orderSum += product.Price;

            orderItem = OrderItem.Create(
                new OrderItemId(Guid.NewGuid()),
                new ProductId(product.Id),
                command.Request.Items[i].Quantity);

            if (orderItem.IsSuccess)
            {
                orderItems.Add(orderItem.Value);
                await usersContracts.UpdateBasketItem(
                    basketItemId, 
                    new UpdateEntityRequestDto("Quantity", basketItem.Quantity - command.Request.Items[i].Quantity),
                    cancellationToken);

                // TODO
                // if (basketItem.Quantity <= 0)
                // {
                //     basket?.RemoveItem(basketItem);
                // }
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

        var order = new Order(
            new OrderId(Guid.NewGuid()),
            new UserId(command.Request.UserId),
            command.Request.DeliveryAddress,
            (PaymentWay)Enum.Parse(typeof(PaymentWay), command.Request.PaymentMethod),
            command.Request.ExpectedTimeDelivery,
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

        await usersContracts.UpdateUser(
            new UserId(user.Id),
            new UpdateEntityRequestDto("BonusPoints", user.BonusPoints - command.Request.UsedBonuses),
            cancellationToken);

        await usersContracts.UpdateUser(
            new UserId(user.Id),
            new UpdateEntityRequestDto("BonusPoints", user.BonusPoints + newBonuses),
            cancellationToken);

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
                    new OrderItemResponseDto(o.ProductId.Value, o.Quantity))
                .ToArray(),
        };

        return response;
    }
}