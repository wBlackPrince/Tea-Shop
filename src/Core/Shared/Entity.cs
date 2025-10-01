using CSharpFunctionalExtensions;

namespace Shared;

public class Entity
{
    public UnitResult<Error> CheckAttributeIsNotEmpty(string attribute)
    {
        if (string.IsNullOrWhiteSpace(attribute))
        {
            return Error.Validation(
                "update.user",
                "attribute cannot be empty");
        }

        return UnitResult.Success<Error>();
    }
}