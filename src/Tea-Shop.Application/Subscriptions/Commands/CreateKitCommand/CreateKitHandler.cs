using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Subscriptions;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Subscriptions.Commands.CreateKitCommand;

public class CreateKitHandler(
    ISubscriptionsRepository subscriptionsRepository,
    IProductsRepository productsRepository,
    ILogger<CreateKitHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<KitDto, CreateKitCommand>
{
    public async Task<Result<KitDto, Error>> Handle(
        CreateKitCommand command,
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

        float sum = 0;
        Product? product;

        for (int i = 0; i < command.Request.Items.Length; i++)
        {
            product = await productsRepository.GetProductById(
                command.Request.Items[i].ProductId,
                cancellationToken);
            if (product is null)
            {
                logger.LogError($"Product with id {command.Request.Items[i].ProductId} not found");
                transactionScope.Rollback();
                return Error.Failure("create.kit", "Product not found");
            }

            sum += (command.Request.Items[i].Amount * product.Price);
        }

        KitId kitId = new KitId(Guid.NewGuid());

        var kitItems = command.Request.Items.Select(ki => new KitItem(
            new KitItemId(Guid.NewGuid()),
            kitId,
            new ProductId(ki.ProductId),
            ki.Amount));


        var kit = new Kit(
            new KitId(Guid.NewGuid()),
            command.Request.Name,
            sum,
            command.Request.Description,
            kitItems);

        await subscriptionsRepository.CreateKit(kit, cancellationToken);

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

        var kitItemsDto = kitItems.Select(ki => new KitItemDto(
            ki.Id.Value,
            ki.KitId.Value,
            ki.ProductId.Value,
            ki.Amount)).ToList();

        var kitDto = new KitDto()
        {
            Id = kit.Id.Value,
            Name = kit.Name,
            AvatarId = kit.AvatarId,
            Description = kit.Details.Description,
            Sum = kit.Details.Sum,
            Items = kitItemsDto,
        };

        return kitDto;
    }
}