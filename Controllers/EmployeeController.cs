using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebApplication2.Dtos.Employee;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeDetailsDto>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDetailsDto>>> GetEmployees(int? departmentId = null,int pageNumber = 1,int pageSize = 10)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            string currentUserId =User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _employeeService.GetEmployeesAsync(
                currentUserId,
                role,
                departmentId,
                pageNumber,
                pageSize);

            if (result == null)
                return Forbid();
            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<EmployeeDetailsDto>> CreateEmployee(CreateEmployeeRequestDto request)
        {
            var result = await _employeeService.AddEmployeeAsync(request);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EmployeeDetailsDto>> UpdateEmployeeData(int id,[FromBody] UpdateEmployeeDto employee)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _employeeService.UpdateEmployeeAsync(id, employee, currentUserId, role);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EmployeeDetailsDto>> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            return Ok(result);
        }
    }
}
