namespace TeaShop.Contract.Tags;

public record CreateTagDto(
    Guid Id,
    string Name,
    string Description);