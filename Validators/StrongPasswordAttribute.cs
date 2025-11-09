using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace inxhsofti.Validators 
{
    public class StrongPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Fjalëkalimi është i detyruar");

            string password = value.ToString();

            if (string.IsNullOrEmpty(password))
                return new ValidationResult("Fjalëkalimi është i detyruar");

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return new ValidationResult("Fjalëkalimi duhet të përmbajë të paktën një shkronjë të madhe");

            if (!Regex.IsMatch(password, @"[0-9]"))
                return new ValidationResult("Fjalëkalimi duhet të përmbajë të paktën një numër");

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
                return new ValidationResult("Fjalëkalimi duhet të përmbajë të paktën një karakter special");

            if (password.Contains("'"))
                return new ValidationResult("Fjalëkalimi nuk mund të përmbajë thonjëza teke");

            return ValidationResult.Success;
        }
    }
}
