using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebApplication2.Dtos.LeaveRequest;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager,Employee")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost("{employeeId}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<LeaveRequestDetalisDto>> CreateLeaveRequest(int employeeId, CreateLeaveRequestDto dto)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _leaveRequestService.AddLeaveRequestAsync( employeeId,  dto,  currentUserId,  role);
            return Created("", result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllLeaveRequestsDto>>> GetLeaveRequests(int pageNumber = 1, int pageSize = 10)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _leaveRequestService.GetAllLeaveRequestsAsync(currentUserId, role, pageNumber, pageSize);
            return Ok(result);
        }
        [HttpGet("{employeeId}")]
        public async Task<ActionResult<IEnumerable<AllLeaveRequestsDto>>> GetLeaveRequestsOfEmployee(int employeeId)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _leaveRequestService.GetLeaveRequestsOfAnEmployeeAsync(employeeId, currentUserId, role);
            return Ok(result);
        }

        [HttpPut("{employeeId}/{leaveRequestId}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<LeaveRequestDetalisDto>> UpdateLeaveRequest(int employeeId, int leaveRequestId, UpdateLeaveRequestDto dto)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _leaveRequestService.UpdateLeaveRequestAsync(employeeId, leaveRequestId, dto, currentUserId, role);
            return Ok(result);
        }

        [HttpDelete("{employeeId}/{leaveRequestId}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<LeaveRequestDetalisDto>> DeleteLeaveRequest(int employeeId, int leaveRequestId)
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _leaveRequestService.DeleteLeaveRequestAsync(employeeId, leaveRequestId, currentUserId, role);
            return Ok(result);
        }
    }
}
