using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is DateTime date && date <= DateTime.Now)
            return new ValidationResult("التاريخ يجب أن يكون في المستقبل");
        return ValidationResult.Success;
    }
}