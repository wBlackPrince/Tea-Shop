using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Contracts.Dtos;
using Products.Domain;
using Shared;
using Shared.ValueObjects;

namespace Products.Application;

public class TagsService : ITagsService
{
    private readonly ITagsRepository _tagsRepository;
    private readonly ILogger<TagsService> _logger;
    private readonly IValidator<CreateTagRequestDto> _tagValidator;

    public TagsService(
        ITagsRepository tagsRepository,
        ILogger<TagsService> logger,
        IValidator<CreateTagRequestDto> tagValidator)
    {
        _tagsRepository = tagsRepository;
        _logger = logger;
        _tagValidator = tagValidator;
    }

    public async Task<Guid> CreateTag(
        CreateTagRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _tagValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        Tag tag = new Tag(
            new TagId(Guid.NewGuid()),
            request.Name,
            request.Description);

        await _tagsRepository.CreateTag(tag, cancellationToken);

        await _tagsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Created tag {tag.Id}");

        return tag.Id.Value;
    }

    public async Task<Result<Guid, Error>> Delete(
        Guid tagId,
        CancellationToken cancellationToken)
    {
        var deletedResult = await _tagsRepository.DeleteTag(new TagId(tagId), cancellationToken);

        if (deletedResult.IsFailure)
        {
            return deletedResult.Error;
        }

        _logger.LogInformation("Deleted tag {tagId}", tagId);

        return tagId;
    }
}