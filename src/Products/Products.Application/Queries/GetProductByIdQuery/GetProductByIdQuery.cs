namespace Products.Application.Queries.GetProductByIdQuery;

public record GetProductByIdQuery(ProductWithOnlyIdDto Request): IQuery;