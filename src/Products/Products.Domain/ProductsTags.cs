namespace Products.Domain;

public record ProductsTagsId(Guid Value);

public class ProductsTags
{
    // для ef core
    private ProductsTags() { }

    public ProductsTags(ProductsTagsId id, Product product, TagId tagId)
    {
        Id = id;
        Product = product;
        TagId = tagId;
    }

    public ProductsTagsId Id { get; set; }

    public Product Product { get; set; }

    public TagId TagId { get; set; }
}