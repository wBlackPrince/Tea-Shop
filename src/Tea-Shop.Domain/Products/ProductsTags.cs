namespace Tea_Shop.Domain.Products;

public class ProductsTags
{
    public ProductsTags(Guid id, Product product, Guid tagId)
    {
        Id = id;
        Product = product;
        TagId = tagId;
    }

    public Guid Id { get; set; }

    public Product Product { get; set; }

    public Guid TagId { get; set; }
}