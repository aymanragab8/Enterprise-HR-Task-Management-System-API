using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dtos.LeaveRequest;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services.Repositories
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<LeaveRequestDetalisDto> AddLeaveRequestAsync(int employeeId, CreateLeaveRequestDto dto, string currentUserId, string role)
        {

            var emp =await _context.Employees.FirstOrDefaultAsync(e=>e.Id==employeeId);

            if (emp == null) throw new KeyNotFoundException("Employee not found.");

            if (role == "Employee")
            {
                var employee =await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);
                if (employee.Id != employeeId)
                    throw new UnauthorizedAccessException("Not Allowed");
            }
            var respone = _mapper.Map<LeaveRequest>(dto);

            await _context.LeaveRequests.AddAsync(respone);
            respone.EmployeeId=employeeId;
            await _context.SaveChangesAsync();

            return _mapper.Map<LeaveRequestDetalisDto>(respone);
        }
        public async Task<IEnumerable<AllLeaveRequestsDto>> GetAllLeaveRequestsAsync(
            string currentUserId,
            string role,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentOutOfRangeException("Invalid pagination parameters.");

            var query = _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => !l.IsDeleted)
                .AsQueryable();

            if (role == "Employee")
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

                if (employee == null)
                    throw new KeyNotFoundException("Employee not found.");

                query = query.Where(l => l.EmployeeId == employee.Id);
            }

            else if (role == "Manager")
            {
                var manager = await _context.Employees
                    .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

                if (manager == null)
                    throw new KeyNotFoundException("Manager not found.");

                query = query.Where(l => l.Employee.DepartmentId == manager.DepartmentId);
            }

            var requests = await query
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AllLeaveRequestsDto>>(requests);
        }

        public async Task<IEnumerable<AllLeaveRequestsDto>> GetLeaveRequestsOfAnEmployeeAsync(int employeeId, string currentUserId, string role)
        {
            var targetEmployee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (targetEmployee == null)
                return Enumerable.Empty<AllLeaveRequestsDto>();

            var leaveRequest = await _context.LeaveRequests.Include(l=>l.Employee).FirstOrDefaultAsync(e => e.Id == employeeId);
            if (role == "Manager")
            {
                var manager = await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);
                if (manager.DepartmentId == targetEmployee.DepartmentId)
                    _mapper.Map<IEnumerable<AllLeaveRequestsDto>>(leaveRequest);
                else return null;
            }
            if (role == "Employee")
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);
                if (employee.Id == employeeId)
                    _mapper.Map<IEnumerable<AllLeaveRequestsDto>>(leaveRequest);

                else return null;
            }

            var requests = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => !l.IsDeleted && l.EmployeeId == employeeId)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<AllLeaveRequestsDto>>(requests);
        }

        public async Task<LeaveRequestDetalisDto> UpdateLeaveRequestAsync(
            int employeeId,
            int leaveRequestId,
            UpdateLeaveRequestDto dto,
            string currentUserId,
            string role)
        {
            var targetEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (targetEmployee == null)
                throw new KeyNotFoundException("Employee not found.");

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l =>
                    l.Id == leaveRequestId &&
                    l.EmployeeId == employeeId);

            if (leaveRequest == null)
                throw new KeyNotFoundException("Leave request not found for this employee.");

            if (role != "Manager")
                throw new UnauthorizedAccessException("Only managers can update leave requests.");

            var manager = await _context.Employees
                .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (manager == null)
                throw new UnauthorizedAccessException("Manager not found.");

            if (manager.DepartmentId != targetEmployee.DepartmentId)
                throw new UnauthorizedAccessException("You can only manage employees in your department.");

            _mapper.Map(dto, leaveRequest);

            await _context.SaveChangesAsync();

            return _mapper.Map<LeaveRequestDetalisDto>(leaveRequest);
        }

        public async Task<LeaveRequestDetalisDto> DeleteLeaveRequestAsync(
            int employeeId,
            int leaveRequestId,
            string currentUserId,
            string role)
        {
            var targetEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (targetEmployee == null)
                throw new KeyNotFoundException("Employee not found.");

            var leaveRequest = await _context.LeaveRequests
                .FirstOrDefaultAsync(l =>
                    l.Id == leaveRequestId &&
                    l.EmployeeId == employeeId);

            if (leaveRequest == null)
                throw new KeyNotFoundException("Leave request not found for this employee.");

            if (role != "Manager")
                throw new UnauthorizedAccessException("Only managers can delete leave requests.");

            var manager = await _context.Employees
                .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (manager == null)
                throw new UnauthorizedAccessException("Manager not found.");

            if (manager.DepartmentId != targetEmployee.DepartmentId)
                throw new UnauthorizedAccessException("You can only manage employees in your department.");

            leaveRequest.IsDeleted = true;
            leaveRequest.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<LeaveRequestDetalisDto>(leaveRequest);
        }

    }
}
