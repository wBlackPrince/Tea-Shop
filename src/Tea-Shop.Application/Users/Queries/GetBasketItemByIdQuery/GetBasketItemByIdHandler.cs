using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetBasketItemByIdQuery;

public class GetBasketItemByIdHandler(
    IReadDbContext dbContext): IQueryHandler<BasketItemDto?, GetBasketItemByIdQuery>
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
            Quantity = basketItem.Quantity,
        };
    }
}