using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dtos.Department;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllDepartmentsDto>>> GetAllDepartments(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var result = await _departmentService
                .GetAllDepartmentsAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentDetalisDto>> GetDepartmentById(int id)
        {
            var data = await _departmentService.GetDepartmentByIdAsync(id);
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DepartmentDetalisDto>> AddDepartment([FromBody] CreateDepartmentDto department)
        {
            var result = await _departmentService.AddDepartmentAsync(department);

            return CreatedAtAction(
                nameof(GetDepartmentById),
                new { id = result.Id },
                result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<DepartmentDetalisDto>> UpdateDepartment(int id,[FromBody] UpdateDepartmentDto department)
        {
            var result = await _departmentService.UpdateDepartmentAsync(id, department);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<DepartmentDetalisDto>> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            return Ok(result);
        }
        [HttpPut("{departmentId}/{managerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DepartmentDetalisDto>> AssignManager(int departmentId, int managerId)
        {
            var result = await _departmentService.AssignManagerAsync(departmentId, managerId);
            return Ok(result);
        }

    }
}
