namespace Orders.Contracts;

public record GetUserOrdersResponseDto
{
    public Guid UserId { get; init; }

    public int TotalOrdersCount { get; init; }

    public int CanceledOrdersCount { get; init; }

    public int DeliveredOrdersCount { get; init; }

    public int NotFinishedOrdersCount { get; init; }

    public List<OrderDto> Orders { get; set; } = [];
}