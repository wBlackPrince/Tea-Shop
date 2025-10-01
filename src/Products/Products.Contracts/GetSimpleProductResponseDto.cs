namespace Products.Contracts;

public record GetSimpleProductResponseDto
{
    public Guid ProductId { get; init; }

    public string ProductName { get; init; }
}