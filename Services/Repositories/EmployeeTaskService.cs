using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication2.Dtos.LeaveRequest;
using WebApplication2.Dtos.Task;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services.Repositories
{
    public class EmployeeTaskService : IEmployeeTaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeTaskService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TaskDetalisDto> AddTaskAsync(int employeeId, AssignTaskDto dto, string currentUserId, string role)
        {
            var assignedEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (assignedEmployee == null)
                throw new KeyNotFoundException("Assigned employee not found.");


            if (role != "Manager")
                throw new UnauthorizedAccessException();

            var manager = await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (manager == null || manager.DepartmentId != assignedEmployee.DepartmentId)
                throw new UnauthorizedAccessException("You can assign tasks only to employees in your department.");

            var task = _mapper.Map<EmployeeTask>(dto);
            task.AssignedToEmployeeId = employeeId;
            task.CreatedByEmployeeId = manager.Id;
            await _context.Tasks.AddAsync(task);

            await _context.SaveChangesAsync();
            return _mapper.Map<TaskDetalisDto>(task);
        }

        public async Task<IEnumerable<AllAssignedTasksDto>> GetAssignedTasksOfAnEmployeeAsync(
            int employeeId,
            string currentUserId,
            string role)
        {
            if (role != "Manager")
                throw new UnauthorizedAccessException("Only managers can view assigned tasks.");

            var targetEmp = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (targetEmp == null)
                throw new KeyNotFoundException("Employee not found.");

            var manager = await _context.Employees
                .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (manager == null)
                throw new UnauthorizedAccessException("Manager not found.");

            if (manager.DepartmentId != targetEmp.DepartmentId)
                throw new UnauthorizedAccessException(
                    "You can view tasks only for employees in your department.");

            var tasks = await _context.Tasks
                .Where(t => t.AssignedToEmployeeId == employeeId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AllAssignedTasksDto>>(tasks);
        }


        public async Task<TaskDetalisDto> UpdateTaskAsync(int employeeId,int taskId,UpdateTaskDetalisDto dto,string currentUserId,string role)
        {
            if (role != "Employee")
                throw new UnauthorizedAccessException("Only employees can update tasks.");

            var emp = await _context.Employees
                .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (emp == null || emp.Id != employeeId)
                throw new UnauthorizedAccessException("You can update only your tasks.");

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t =>
                    t.Id == taskId &&
                    t.AssignedToEmployeeId == employeeId);

            if (task == null)
                throw new KeyNotFoundException("Task not found for this employee.");

            _mapper.Map(dto, task);

            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskDetalisDto>(task);
        }


        public async Task<TaskDetalisDto> DeleteTaskAsync(int employeeId,int taskId,string currentUserId,string role)
        {
            var targetEmployee = await _context.Employees.Include(e=>e.DepartmentId)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (targetEmployee == null)
                throw new KeyNotFoundException("Employee not found.");

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t =>
                    t.Id == taskId &&
                    t.AssignedToEmployeeId == employeeId);

            if (task == null)
                throw new KeyNotFoundException("Task not found for this employee.");

            if (role != "Manager")
                throw new UnauthorizedAccessException("Only managers can delete Tasks.");

            var manager = await _context.Employees
                .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (manager == null)
                throw new UnauthorizedAccessException("Manager not found.");

            if (manager.DepartmentId != targetEmployee.DepartmentId)
                throw new UnauthorizedAccessException("You can only manage employees in your department.");

            task.IsDeleted = true;
            await _context.SaveChangesAsync();
            return _mapper.Map<TaskDetalisDto>(task);
        }
    }
}
