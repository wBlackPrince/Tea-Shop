using System.Data;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Subscriptions.Commands.CreateOrderBasedOnSubscriptionCommand;

public class CreateOrderBasedOnSubscriptionHandler(
    IUsersRepository usersRepository,
    IOrdersRepository ordersRepository,
    IReadDbContext readDbContext,
    IDbConnectionFactory dbConnectionFactory,
    ISubscriptionsRepository subscriptionsRepository,
    ILogger<CreateOrderBasedOnSubscriptionHandler> logger,
    ITransactionManager transactionManager):
    ICommandHandler<CreateOrderResponseDto, CreateOrderBasedOnSubscriptionCommand>
{
    public async Task<Result<CreateOrderResponseDto, Error>> Handle(
        CreateOrderBasedOnSubscriptionCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating user");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var userId = new UserId(command.Request.UserId);
        var user = await usersRepository.GetUserById(userId, cancellationToken);

        var subscriptionId = new SubscriptionId(command.Request.SubscriptionId);
        var subscriptionWithKit = await subscriptionsRepository
            .GetSubscriptionWithKit(subscriptionId, cancellationToken);

        var orderItems = subscriptionWithKit.Kit.KitItems
            .Select(ki => OrderItem.Create(
                new OrderItemId(Guid.NewGuid()),
                ki.ProductId,
                ki.Amount).Value).ToList();

        string? adress = (command.Request.DeliveryAddress ?? user.Address);

        if (adress is null)
        {
            var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);

            var lastOrderDeliveryPlace = await connection.QueryAsync<string>(
                """
                select o.delivery_address
                from users as u join orders as o on u.id = o.user_id
                where u.id = @userId
                order by updated_at desc
                limit 1
                """,
                param: new { userId = command.Request.UserId });

            adress = lastOrderDeliveryPlace.First();
        }

        if (adress is null)
        {
            logger.LogError("No delivery address found for subscription's order");
            transactionScope.Rollback();
            return Error.NotFound(
                "create.order",
                "no delivery address found");
        }

        DateTime createdAt = DateTime.UtcNow;
        DateTime updatedAt = DateTime.UtcNow;
        DateTime expectedDate = CalculExpectedDeliveryTime.Calcul(createdAt, adress, DeliveryWay.Courier);
        DeliveryWay deliveryWay = (DeliveryWay)Enum.Parse(typeof(DeliveryWay), command.Request.DeliveryWay);

        var orderId = new OrderId(Guid.NewGuid());
        var order = new Order(
            orderId,
            userId,
            user.Address,
            PaymentWay.CardOnline,
            deliveryWay,
            expectedDate,
            orderItems,
            createdAt,
            updatedAt);

        await ordersRepository.CreateOrder(order, cancellationToken);


        var saveResult = await transactionManager.SaveChangesAsync(cancellationToken);

        if (saveResult.IsFailure)
        {
            logger.LogError(saveResult.Error.ToString());
            transactionScope.Rollback();
            return saveResult.Error;
        }


        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating user");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        float orderSum = 0;

        foreach (var orderItem in order.OrderItems)
        {
            var product = await readDbContext.ProductsRead
                .FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);

            orderSum += product.Price * orderItem.Quantity;
        }

        return new CreateOrderResponseDto(){
            Id = order.Id.Value,
            UserId = order.UserId.Value,
            DeliveryAddress = order.DeliveryAddress,
            PaymentMethod = order.PaymentWay.ToString(),
            Status = order.OrderStatus.ToString(),
            ExpectedTimeDelivery = order.ExpectedDeliveryTime,
            OrderSum = orderSum,
            Items = orderItems.Select(oi => new OrderItemResponseDto(){
                ProductId = oi.ProductId.Value,
                Quantity = oi.Quantity,
            }).ToArray(),
        };
    }
}