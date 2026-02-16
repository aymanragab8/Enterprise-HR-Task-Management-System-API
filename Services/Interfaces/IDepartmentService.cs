using WebApplication2.Dtos.Common;
using WebApplication2.Dtos.Department;

namespace WebApplication2.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<AllDepartmentsDto>> GetAllDepartmentsAsync(int PageNumber = 1, int PageSize = 10);

        Task<DepartmentDetalisDto> GetDepartmentByIdAsync(int id);

        Task<DepartmentDetalisDto> AddDepartmentAsync(CreateDepartmentDto department);

        Task<DepartmentDetalisDto> UpdateDepartmentAsync(int id, UpdateDepartmentDto department);

        Task<DepartmentDetalisDto> DeleteDepartmentAsync(int id);
        Task<DepartmentDetalisDto> AssignManagerAsync(int departmentId, int managerId);
    }
}
