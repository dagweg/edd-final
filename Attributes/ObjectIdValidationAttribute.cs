using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace HouseRentalSystem.Attributes;

public class ObjectIdValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult("Id is required.");
        }

        var stringRepr = value.ToString();
        var isValid = ObjectId.TryParse(stringRepr, out _);

        return isValid
            ? ValidationResult.Success
            : new ValidationResult("Id is not a valid ObjectId.");
    }
}
