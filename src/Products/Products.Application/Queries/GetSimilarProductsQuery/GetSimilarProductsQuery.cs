namespace Products.Application.Queries.GetSimilarProductsQuery;

public record GetSimilarProductsQuery(ProductWithOnlyIdDto Request): IQuery;