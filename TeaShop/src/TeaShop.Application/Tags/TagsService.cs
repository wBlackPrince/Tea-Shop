using FluentValidation;
using Microsoft.Extensions.Logging;
using TeaShop.Contract.Tags;
using TeaShopDomain.Tags;

namespace TeaShop.Application.Tags;

public class TagsService: ITagsService
{
    private readonly ITagsRepository _tagsRepository;
    private readonly IValidator<CreateTagDto> _CreateTagValidator;
    private readonly ILogger<TagsService> _logger;

    public TagsService(
        ITagsRepository tagsRepository,
        IValidator<CreateTagDto> createTagValidator,
        ILogger<TagsService> logger)
    {
        _tagsRepository = tagsRepository;
        _CreateTagValidator = createTagValidator;
        _logger = logger;
    }

    public async Task<Guid> Create(CreateTagDto request, CancellationToken cancellationToken)
    {
        var result = await _CreateTagValidator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        Guid id = Guid.NewGuid();
        Tag tag = new Tag(id, request.Name, request.Description);

        _tagsRepository.AddAsync(tag, cancellationToken);

        _logger.LogInformation($"Created tag with id: {id}");

        return id;
    }
}