using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;

public record PreparationMethod
{
    private PreparationMethod(){}

    private PreparationMethod(
        int preparationTime,
        string description,
        List<Ingrendient> ingredients)
    {
        PreparationTime = preparationTime;
        Description = description;
        Ingredients = ingredients;
    }

    public int PreparationTime { get; }

    public string Description { get; }

    public List<Ingrendient> Ingredients { get; }

    public static Result<PreparationMethod, Error> Create(
        int preparationTime,
        string description,
        List<Ingrendient> ingredients)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Error.Validation("product.preparationMethod", "preparationTime must be a positive number");


        if (preparationTime < 0)
            return Error.Validation("product.preparationMethod", "preparationTime must be a positive number");

        if (description.Length > Constants.Limit100)
        {
            return Error.Validation("product.preparationMethod", "description must be less than 100 characters");
        }


        return new PreparationMethod(preparationTime, description, ingredients);
    }
}