using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetBasketItemByIdQuery;

public class GetBasketItemHandler(
    IUsersReadDbContext dbContext): IQueryHandler<BasketItemDto?, GetBasketItemByIdQuery>
{
    public async Task<BasketItemDto?> Handle(GetBasketItemByIdQuery query, CancellationToken cancellationToken)
    {
        var basketItem = await dbContext.BasketsItemsRead.FirstOrDefaultAsync(bi => bi.Id == query.BasketId);

        if (basketItem is null)
        {
            return null;
        }

        return new BasketItemDto()
        {
            Id = basketItem.Id.Value,
            BasketId = basketItem.BasketId.Value,
            ProductId = basketItem.ProductId.Value,
            Quantity = basketItem.Quantity
        };
    }
}