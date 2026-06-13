using System.ComponentModel.DataAnnotations;
using EmployeeValidationDemo.Validation;

namespace EmployeeValidationDemo.DTOs
{
    public class LeaveRequestCreateDto
    {
        [Required]
        [StringLength(100,MinimumLength =3)]
        public string EmployeeName {  get; set; }
        [Required]
        [EmailAddress]
        public string EmployeeEmail{ get; set; }
        [Required]
        [RegularExpression(@"^6-9\d{9}$",ErrorMessage ="Enter valid Phone number")]
        public string MobileNumber {  get; set; }
        [LeaveTypeValidation]
        public string LeaveType { get; set; }
        [FutureDateValidation]
        public DateTime StartDate { get; set; }
        [FutureDateValidation]
        public DateTime EndDate { get; set; }
        [Required]
        [StringLength(250,MinimumLength =10)]
        public string Reason {  get; set; }
    }
}
