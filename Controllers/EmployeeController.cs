using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dtos.Employee;
using WebApplication2.Models;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IMapper mapper ,IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<ResponseEmployeeDto>> GetEmployeeById(int id)
        { 
            var employee =await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseEmployeeDto>>> GetAllEmployees(int PageNumber = 1 ,int pageSize = 10)
        {
            var result = await _employeeService.GetAllEmployeesAsync(PageNumber, pageSize);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ResponseEmployeeDto>> CreateNewEmployee(CreateEmployeeDto employee)
        {
            var result = await _employeeService.CreateEmployeeAsync(employee);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult<ResponseEmployeeDto>> UpdateEmployeeData(int id ,CreateEmployeeDto employee)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id ,employee);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult<ResponseEmployeeDto>> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            return Ok(result);
        }

    }
}
