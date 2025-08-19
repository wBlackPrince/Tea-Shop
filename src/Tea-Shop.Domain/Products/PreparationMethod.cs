using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;

public record PreparationMethod
{
    private PreparationMethod(int preparationTime, string description)
    {
        PreparationTime = preparationTime;
        Description = description;
    }

    public int PreparationTime { get; }

    public string Description { get; }

    public static Result<PreparationMethod, Error> Create(int preparationTime, string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Error.Validation("product.preparationMethod", "preparationTime must be a positive number");


        if (preparationTime < 0)
            return Error.Validation("product.preparationMethod", "preparationTime must be a positive number");

        if (description.Length > Constants.Limit100)
        {
            return Error.Validation("product.preparationMethod", "description must be less than 100 characters");
        }


        return new PreparationMethod(preparationTime, description);
    }
}