using EmployeeValidationDemo.DTOs;

namespace EmployeeValidationDemo.Services
{
    public interface ILeaveRequestService
    {
        LeaveRequestResponseDto CreateDto(LeaveRequestCreateDto dto);
        IEnumerable<LeaveRequestResponseDto> GetAll();
        LeaveRequestResponseDto GetById(int id);
    }
}
