using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;

public record PreparationMethod
{
    private PreparationMethod(
        int preparationTime,
        string description,
        List<Ingrendient> ingredients)
    {
        PreparationTime = preparationTime;
        Description = description;
        Ingredients = ingredients;
    }

    private PreparationMethod()
    {
    }

    public int PreparationTime { get; private set; }

    public string Description { get; private set; }

    public List<Ingrendient> Ingredients { get; set; }

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

    public UnitResult<Error> UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return Error.Validation(
                "update.preparation_time",
                "description must not be empty");
        }

        if (description.Length > Constants.Limit1000 || description.Length < Constants.Limit2)
        {
            return Error.Validation(
                "update.preparation_description",
                "description must be less than 1000 characters or greater than 1 character");
        }

        Description = description;

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> UpdatePreparationTime(int? preparationTime)
    {
        if (preparationTime is null)
        {
            return Error.Validation(
                "update.preparation_time",
                "preparation time must not be null");
        }

        if (preparationTime.Value < 0)
        {
            return Error.Validation(
                "update.preparation_time",
                "preparation time must be a positive number");
        }

        PreparationTime = preparationTime.Value;

        return UnitResult.Success<Error>();
    }
}