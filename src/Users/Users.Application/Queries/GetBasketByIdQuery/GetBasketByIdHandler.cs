using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetBasketByIdQuery;

public class GetBasketByIdHandler(IUsersReadDbContext dbContext): IQueryHandler<BasketDto?, GetBasketByIdQuery>
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