using System.Data;
using CSharpFunctionalExtensions;
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

// public class CreateOrderBasedOnSubscriptionHandler(
//     IUsersRepository usersRepository,
//     IOrdersRepository ordersRepository,
//     ISubscriptionsRepository subscriptionsRepository,
//     ILogger<CreateOrderBasedOnSubscriptionHandler> logger,
//     ITransactionManager transactionManager):
//     ICommandHandler<CreateOrderResponseDto, CreateOrderBasedOnSubscriptionCommand>
// {
//     public async Task<Result<CreateOrderResponseDto, Error>> Handle(
//         CreateOrderBasedOnSubscriptionCommand command,
//         CancellationToken cancellationToken)
//     {
//         var transactionScopeResult = await transactionManager.BeginTransactionAsync(
//             IsolationLevel.RepeatableRead,
//             cancellationToken);
//
//         if (transactionScopeResult.IsFailure)
//         {
//             logger.LogError("Failed to begin transaction while creating user");
//             return transactionScopeResult.Error;
//         }
//
//         using var transactionScope = transactionScopeResult.Value;
//
//         var userId = new UserId(command.Request.UserId);
//         var user = await usersRepository.GetUserById(userId, cancellationToken);
//
//         var subscriptionId = new SubscriptionId(command.Request.SubscriptionId);
//         var subscription = await subscriptionsRepository
//             .GetSubscriptionWithKit(subscriptionId, cancellationToken);
//
//         var orderId = new OrderId(Guid.NewGuid());
//         var order = new Order(
//             orderId,
//             userId,
//             user.Address,
//             PaymentWay.CardOnline,
//             
//             )
//
//
//         var saveResult = await transactionManager.SaveChangesAsync(cancellationToken);
//
//         if (saveResult.IsFailure)
//         {
//             logger.LogError(saveResult.Error.ToString());
//             transactionScope.Rollback();
//             return saveResult.Error;
//         }
//
//
//         var commitedResult = transactionScope.Commit();
//
//         if (commitedResult.IsFailure)
//         {
//             logger.LogError("Failed to commit result while creating user");
//             transactionScope.Rollback();
//             return commitedResult.Error;
//         }
//     }
// }