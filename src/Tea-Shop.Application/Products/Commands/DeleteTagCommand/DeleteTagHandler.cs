using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.DeleteTagCommand;

public class DeleteTagHandler(
    IProductsRepository productsRepository,
    ILogger<DeleteTagHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<Guid, DeleteTagCommand>
{
    public async Task<Result<Guid, Error>> Handle(
        DeleteTagCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        var deletedResult = await productsRepository.DeleteTag(
            new TagId(command.TagId),
            cancellationToken);

        if (deletedResult.IsFailure)
        {
            return deletedResult.Error;
        }


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating tag");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogInformation("Deleted tag {tagId}", command.TagId);

        return command.TagId;
    }
}