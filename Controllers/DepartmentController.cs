using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dtos.Department;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService , IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseDepartmentDto>>> GetAllDepartments(int PageNumber=1 , int PageSize =10)
        {
            var result =await _departmentService.GetAllDepartmentsAsync(PageNumber, PageSize);
            return Ok(result);
        }

        //[HttpGet ("Count")]
        //public IActionResult GetDepartmentsEmpCount()
        //{
        //    var deptlist = _context.Departments.Include(d => d.Employees).ToList();
        //    if (!deptlist.Any()) 
        //    {
        //        return NotFound("Data Not Found");
        //    }
        //    var dtolist = new List<DeptWithEmpCountDto>();
        //    foreach (var dept in deptlist)
        //    {
        //        var dto = new DeptWithEmpCountDto
        //        {
        //            Id = dept.Id,
        //            Name = dept.Name,
        //            EmployeeCount = dept.Employees.Count()
        //        };
        //        dtolist.Add(dto);
        //    }
        //    return Ok(dtolist);
        //}


        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseDepartmentDto>> GetDepartmentById(int id)
        {
            var data =await _departmentService.GetDepartmentByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDepartmentDto>> AddDepartment(CreateDepartmentDto department)
        {
           var result =await _departmentService.AddDepartmentAsync(department);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<ResponseDepartmentDto>> UpdateDepartment(int id, CreateDepartmentDto department)
        {
            var result =await _departmentService.UpdateDepartmentAsync(id, department);
            return Ok(result);
        }     

        [HttpDelete]
        public async Task<ActionResult<ResponseDepartmentDto>> DeleteDepartment(int id)
        {
            var result=await _departmentService.DeleteDepartmentAsync(id);
            return Ok(result);
        }

    }
}
