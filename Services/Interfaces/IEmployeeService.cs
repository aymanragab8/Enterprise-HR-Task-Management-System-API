using WebApplication2.Dtos.Common;
using WebApplication2.Dtos.Employee;

namespace WebApplication2.Services.Interfaces
{
    public interface IEmployeeService
    {
            Task<IEnumerable<AllEmployeesDto>> GetEmployeesAsync(string currentUserId,string role, int? departmentId = null,int pageNumber = 1,int pageSize = 10);
            Task<EmployeeDetailsDto> GetEmployeeByIdAsync(int id, string role, string currentUserId);
            Task<EmployeeDetailsDto> AddEmployeeAsync(CreateEmployeeRequestDto request);
            Task<EmployeeDetailsDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto employee, string currentUserId, string role);
            Task<EmployeeDetailsDto> DeleteEmployeeAsync(int id);   
    }
}
