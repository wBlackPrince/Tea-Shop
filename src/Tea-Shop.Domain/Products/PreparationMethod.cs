using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Products;

public record PreparationMethod
{
    private string _description;
    private int _preparationTime;

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

    public int PreparationTime
    {
        get => _preparationTime;
        set => UpdatePreparationTime(value);
    }

    public string Description
    {
        get => _description;
        set => UpdateDescription(value);
    }

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

    public void UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ValidationException("description must not be empty");

        if (description.Length > Constants.Limit1000 || description.Length < Constants.Limit2)
            throw new ValidationException("description must be less than 1000 characters or greater than 1 character");

        _description = description;
    }

    public void UpdatePreparationTime(int? preparationTime)
    {
        if (preparationTime is null)
            throw new ValidationException("preparation time must not be null");

        if (preparationTime.Value < 0)
            throw new ValidationException("preparation time must be a positive number");

        _preparationTime = preparationTime.Value;
    }
}