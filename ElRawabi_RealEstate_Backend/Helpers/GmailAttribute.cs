using System.ComponentModel.DataAnnotations;

namespace ElRawabi_RealEstate_Backend.Helpers
{
    public class GmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult("البريد الإلكتروني مطلوب.");

            if (!email.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
                return new ValidationResult("يجب أن يكون البريد الإلكتروني بصيغة @gmail.com فقط.");

            return ValidationResult.Success;
        }
    }
}
