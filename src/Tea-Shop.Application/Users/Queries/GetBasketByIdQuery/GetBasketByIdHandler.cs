using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetBasketByIdQuery;

public class GetBasketByIdHandler(IReadDbContext dbContext): IQueryHandler<BasketDto?, GetBasketByIdQuery>
{
    public async Task<BasketDto?> Handle(GetBasketByIdQuery query, CancellationToken cancellationToken)
    {
        var basket = await dbContext.BasketsRead
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == query.BasketId, cancellationToken);

        if (basket is null)
        {
            return null;
        }

        return new BasketDto()
        {
            Id = basket.Id.Value,
            UserId = basket.UserId.Value,
            Items = basket.Items.Select(bi => new BasketItemDto()
            {
                Id = bi.Id.Value,
                BasketId = bi.BasketId.Value,
                ProductId = bi.ProductId.Value,
                Quantity = bi.Quantity,
            }).ToList()
        };
    }
}