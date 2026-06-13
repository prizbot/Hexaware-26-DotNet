using EmployeeValidationDemo.DTOs;
using EmployeeValidationDemo.Models;
using EmployeeValidationDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeValidationDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }
        [HttpPost]
        public ActionResult<LeaveRequestResponseDto> Create(LeaveRequestCreateDto dto)
        {
            try
            {
                var res = _leaveRequestService.CreateDto(dto);
                return CreatedAtAction(nameof(GetById), new { id = res.LeaveRequestId }, res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public ActionResult<IEnumerable<LeaveRequestResponseDto>> GetAll()
        {
            return Ok(_leaveRequestService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult<LeaveRequestResponseDto>GetById(int id)
        {
            var req=_leaveRequestService.GetById(id);
            if(req== null) return NotFound();
            return Ok(req);
        }
    }
}
