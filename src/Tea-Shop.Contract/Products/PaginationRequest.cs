namespace Tea_Shop.Contract.Products;

public record PaginationRequest(
    int Page = 1,
    int PageSize = 20);