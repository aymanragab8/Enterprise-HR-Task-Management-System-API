using WebApplication2.Dtos.Task;

namespace WebApplication2.Services.Interfaces
{
    public interface IEmployeeTaskService
    {
        Task<IEnumerable<AllAssignedTasksDto>> GetAssignedTasksOfAnEmployeeAsync(int employeeId,string currentUserId,string role);

        Task<TaskDetalisDto> AddTaskAsync(int employeeId, AssignTaskDto dto, string currentUserId, string role);

        Task<TaskDetalisDto> UpdateTaskAsync(int employeeId, int taskId, UpdateTaskDetalisDto dto, string currentUserId, string role);

        Task<TaskDetalisDto> DeleteTaskAsync(int employeeId, int taskId, string currentUserId, string role);
    }
}
