using WebApplication2.Dtos.Department;

namespace WebApplication2.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<ResponseDepartmentDto>> GetAllDepartmentsAsync(int pageNumber = 1, int pageSize = 10);

        Task<ResponseDepartmentDto> GetDepartmentByIdAsync(int id);

        Task<ResponseDepartmentDto> AddDepartmentAsync(CreateDepartmentDto department);

        Task<ResponseDepartmentDto> UpdateDepartmentAsync(int id, CreateDepartmentDto department);

        Task<ResponseDepartmentDto> DeleteDepartmentAsync(int id);
    }
}
