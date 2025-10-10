using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.CreateTagCommand;

public class CreateTagHandler(
    IProductsRepository productsRepository,
    ITransactionManager transactionManager,
    ILogger<CreateTagHandler> logger,
    IValidator<CreateTagRequestDto> validator): ICommandHandler<Guid, CreateTagCommand>
{
    public async Task<Result<Guid, Error>> Handle(
        CreateTagCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return Error.Validation(
                "tag.create",
                "validation errors",
                validationResult.Errors.First().PropertyName);
        }

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        Tag tag = new Tag(
            new TagId(Guid.NewGuid()),
            command.Request.Name,
            command.Request.Description);

        await productsRepository.CreateTag(tag, cancellationToken);



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating tag");
            transactionScope.Rollback();
            return commitedResult.Error;
        }



        logger.LogInformation($"Created tag {tag.Id}");

        return tag.Id.Value;
    }
}