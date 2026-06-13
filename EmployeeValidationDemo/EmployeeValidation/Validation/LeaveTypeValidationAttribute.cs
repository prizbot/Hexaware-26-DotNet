using System.ComponentModel.DataAnnotations;

namespace EmployeeValidationDemo.Validation
{
    public class LeaveTypeValidationAttribute:ValidationAttribute
    {
        private readonly string[] _validType =
        {
            "Sick","Casual","Earned"
        };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return new ValidationResult("Leave type is required");
            }
            if(!_validType.Contains(value.ToString()))
            {
                return new ValidationResult($"LeaveType must be one of: {string.Join(",", _validType)}");
            }
            return ValidationResult.Success;
        }

    }
}
