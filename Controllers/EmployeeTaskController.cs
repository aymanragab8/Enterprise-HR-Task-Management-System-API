using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication2.Dtos.Task;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager,Employee")]
    public class EmployeeTaskController : ControllerBase
    {
        private readonly IEmployeeTaskService _taskService;

        public EmployeeTaskController(IEmployeeTaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<TaskDetalisDto>> AddTask(int employeeId,AssignTaskDto dto)
        {
            string currentUserId =User.FindFirst(ClaimTypes.NameIdentifier)!.Value; 
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            var result =await  _taskService.AddTaskAsync(employeeId, dto, currentUserId, role);
            return Created("", result);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<AllAssignedTasksDto>>> GetTasks(int employeeId)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            var result = await _taskService.GetAssignedTasksOfAnEmployeeAsync(employeeId, currentUserId, role);
            return Ok(result);
        }

        [HttpPut("{employeeId}/{taskId}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<TaskDetalisDto>> UpdateTask(int employeeId, int taskId, UpdateTaskDetalisDto dto)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            var result = await _taskService.UpdateTaskAsync(employeeId, taskId, dto, currentUserId, role);
            return Ok(result);
        }

        [HttpDelete("{employeeId}/{taskId}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<TaskDetalisDto>> DeleteTask(int employeeId, int taskId)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = await _taskService.DeleteTaskAsync(employeeId,taskId, currentUserId, role);
            return Ok(result);
        }
    }
}
