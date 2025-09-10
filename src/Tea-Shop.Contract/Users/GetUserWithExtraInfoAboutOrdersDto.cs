namespace Tea_Shop.Contract.Users;

public record GetUserWithExtraInfoAboutOrdersDto
{
    Guid UserId { get; init; }

    int TotalOrdersCount { get; init; }

    int CanceledOrdersCount { get; init; }

    int DeliveredOrdersCount { get; init; }

    int NotFinishedOrdersCount { get; init; }
}