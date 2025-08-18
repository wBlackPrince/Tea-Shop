namespace Tea_Shop.Domain.Products;

public class ProductsIngrendients
{

    public ProductsIngrendients(Guid id, Product product, Ingrendient ingredient)
    {
        Id = id;
        Product = product;
        Ingrendient = ingredient;
    }

    public Guid Id { get; set; }

    public Product Product { get; set; }

    public Ingrendient Ingrendient { get; set; }
}