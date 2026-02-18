using WebApplication2.Dtos.Department;
using WebApplication2.Dtos.LeaveRequest;

namespace WebApplication2.Services.Interfaces
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<AllLeaveRequestsDto>> GetAllLeaveRequestsAsync
            (string currentUserId,
            string role,
            int pageNumber = 1,
            int pageSize = 10);

        Task<IEnumerable<AllLeaveRequestsDto>> GetLeaveRequestsOfAnEmployeeAsync(int employeeId, string currentUserId, string role);

        Task<LeaveRequestDetalisDto> AddLeaveRequestAsync(int employeeId, CreateLeaveRequestDto dto, string currentUserId, string role);

        Task<LeaveRequestDetalisDto> UpdateLeaveRequestAsync(int employeeId, int leaveRequestId, UpdateLeaveRequestDto dto, string currentUserId, string role);

        Task<LeaveRequestDetalisDto> DeleteLeaveRequestAsync(int employeeId, int leaveRequestId, string currentUserId, string role);
    }
}
