using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Validation;

namespace EmployeeValidationDemo.Validation
{
    public class FutureDateValidationAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Date is required");
            DateTime date = (DateTime)value;
            if(date.Date<=DateTime.Today)
            {
                return new ValidationResult("Date must be future date");
            }
            return ValidationResult.Success;
        }

    }
}
