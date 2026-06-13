using EmployeeValidationDemo.DTOs;
using EmployeeValidationDemo.Models;

namespace EmployeeValidationDemo.Services
{
    public class LeaveRequestService:ILeaveRequestService
    {
        private static readonly List<LeaveRequest> _requests = new();
        private static int _id = 1;
        public static LeaveRequestResponseDto Map(LeaveRequest request)
        {
            return new LeaveRequestResponseDto
            {
                LeaveRequestId=request.LeaveRequestId,
                EmployeeName=request.EmployeeName,
                EmployeeEmail=request.EmployeeEmail,
                LeaveType=request.LeaveType,
                StartDate=request.StartDate,
                EndDate=request.EndDate,
                Reason=request.Reason,
                TotalDays=request.TotalDays,
                Status=request.Status,
                CreatedOn=request.CreatedOn
            };
        }
        public LeaveRequestResponseDto CreateDto(LeaveRequestCreateDto dto)
        {
            int totaldays = (dto.EndDate.Date - dto.StartDate.Date).Days + 1;
            if (totaldays <= 0) throw new Exception("EndDate must be greater than StartDate");
            LeaveRequest lr = new LeaveRequest
            {
                LeaveRequestId = _id++,
                EmployeeName = dto.EmployeeName,
                EmployeeEmail = dto.EmployeeEmail,
                MobileNumber = dto.MobileNumber,
                LeaveType = dto.LeaveType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                TotalDays = totaldays,
                Status = "Pending",
                CreatedOn = DateTime.UtcNow
            };
            _requests.Add(lr);
            return Map(lr);

        }
        public IEnumerable<LeaveRequestResponseDto> GetAll()
        {
            return _requests.Select(Map);

        }
        public LeaveRequestResponseDto GetById(int id)
        {
            var req = _requests.FirstOrDefault(x => x.LeaveRequestId == id);
            if (req == null) return null;
            return Map(req);
        }


        
    }
}
