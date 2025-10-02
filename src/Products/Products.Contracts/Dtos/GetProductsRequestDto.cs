using Shared.Dto;

namespace Products.Contracts.Dtos;

public record GetProductsRequestDto(
    string? Search,
    Guid? TagId,
    string? Season,
    float? MinPrice,
    float? MaxPrice,
    PaginationRequest Pagination,
    string? OrderBy,
    string? OrderDirection);