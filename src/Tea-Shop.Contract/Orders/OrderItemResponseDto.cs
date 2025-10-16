namespace Tea_Shop.Contract.Orders;

public record OrderItemResponseDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
}