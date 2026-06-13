using System.ComponentModel.DataAnnotations;

namespace EmployeeValidationDemo.DTOs
{
    public class LeaveRequestResponseDto
    {
        [Required]
        public int LeaveRequestId {  get; set; }
        [Required]
        public string EmployeeName {  get; set; }
        [Required]
        [EmailAddress]
        public string EmployeeEmail {  get; set; }

        [Required]
        public string LeaveType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Reason { get; set; }
        [Required]
        public int TotalDays { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
    
}
}
