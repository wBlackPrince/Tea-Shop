namespace Tea_Shop.Domain.Products;

public record ProductsTagsId(Guid Value);

public class ProductsTags
{
    // для ef core
    private ProductsTags() { }

    public ProductsTags(ProductsTagsId id, Product product, Guid tagId)
    {
        Id = id;
        Product = product;
        TagId = tagId;
    }

    public ProductsTagsId Id { get; set; }

    public Product Product { get; set; }

    public Guid TagId { get; set; }
}