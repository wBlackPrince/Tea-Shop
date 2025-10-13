using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Subscriptions;

namespace Tea_Shop.Application.Subscriptions.Queries.GetKitByIdQuery;

public class GetKitByIdHandler(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<GetKitByIdHandler> logger): IQueryHandler<KitDto?, GetKitByIdQuery>
{
    public async Task<KitDto?> Handle(GetKitByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetKitByIdHandler));

        using var connection = await dbConnectionFactory.CreateConnectionAsync(
            cancellationToken: cancellationToken);

        KitDto? kitDto = null;

        await connection.QueryAsync<KitDto, KitItemDto, KitDto>(
            """
            select
                k.id as id,
                k.name as name,
                k.avatar_id as avatar_id,
                kd.description as description,
                kd.sum as sum,
                ki.id as kit_item_id,
                ki.kit_id as kit_id,
                ki.product_id as product_id,
                ki.amount as amount
            from kits as k 
                join kits_details as kd on k.id = kd.kit_id
                join kit_items as ki on ki.kit_id = k.id
            where k.id = @kitId
            """,
            param: new
            {
                kitId = query.Request.KitId,
            },
            splitOn: "kit_item_id",
            map: (k, kd) =>
            {
                if (kitDto is null)
                {
                    kitDto = k;
                }

                kitDto.Items.Add(kd);

                return k;
            });

        if (kitDto is null)
        {
            logger.LogWarning("Popular products not found.");
        }

        logger.LogDebug("Get popular products.");

        return kitDto;
    }
}