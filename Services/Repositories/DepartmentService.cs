using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dtos.Department;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DepartmentDetalisDto> AddDepartmentAsync(CreateDepartmentDto department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            var dept = _mapper.Map<Department>(department);
            await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<DepartmentDetalisDto>(dept);
            return result;
        }

        public async Task<DepartmentDetalisDto> DeleteDepartmentAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");

            var dept =await _context.Departments.FindAsync(id);
            if (dept == null)
                throw new KeyNotFoundException("Department Not Found");

            
            dept.IsDeleted=true;
            await _context.SaveChangesAsync();

            return _mapper.Map<DepartmentDetalisDto>(dept);
        }

        public async Task<IEnumerable<AllDepartmentsDto>> GetAllDepartmentsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Invalid pagination parameters.");

            var departments = await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<AllDepartmentsDto>>(departments);
            return result;
        }

        public async Task<DepartmentDetalisDto> GetDepartmentByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id.");

            var department = await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");

            var result = _mapper.Map<DepartmentDetalisDto>(department);
            return result;
        }

        public async Task<DepartmentDetalisDto> UpdateDepartmentAsync(int id, UpdateDepartmentDto departmentDto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id.");

            if (departmentDto == null)
                throw new ArgumentNullException(nameof(departmentDto));

            var entity = await _context.Departments.Include(d=>d.Manager).FirstOrDefaultAsync(d=>d.Id==id);

            if (entity == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");


            _mapper.Map(departmentDto, entity);
            await _context.SaveChangesAsync();
            var result = _mapper.Map<DepartmentDetalisDto>(entity);

            return result;
        }
        public async Task<DepartmentDetalisDto> AssignManagerAsync(int departmentId, int managerId)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == departmentId);

            if (department == null)
                throw new KeyNotFoundException("Department not found.");

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == managerId);

            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            if (employee.DepartmentId != departmentId)
                throw new ArgumentException("Employee must belong to the same department.");

            department.ManagerId = managerId;

            await _context.SaveChangesAsync();

            var result = await _context.Departments
                .Where(d => d.Id == departmentId)
                .Select(d => new DepartmentDetalisDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    ManagerName = d.Manager != null ? d.Manager.Name : null ,
                    Code = d.Code
                })
                .FirstOrDefaultAsync();

            return result!;
        }
    }
}
