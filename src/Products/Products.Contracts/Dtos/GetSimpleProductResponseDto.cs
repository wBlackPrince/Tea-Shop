namespace Products.Contracts.Dtos;

public record GetSimpleProductResponseDto
{
    public Guid ProductId { get; init; }

    public string ProductName { get; init; }
}