using WebApplication2.Dtos.Employee;

namespace WebApplication2.Services.Interfaces
{
    public interface IEmployeeService
    {
            Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployeesAsync(int PageNumber , int PageSize);
            Task<ResponseEmployeeDto> GetEmployeeByIdAsync(int id);
            Task<ResponseEmployeeDto> CreateEmployeeAsync(CreateEmployeeDto employee);
            Task<ResponseEmployeeDto> UpdateEmployeeAsync(int id, CreateEmployeeDto employee);
            Task<ResponseEmployeeDto> DeleteEmployeeAsync(int id);   
    }
}
