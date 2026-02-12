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

        // ============================
        // Add
        // ============================
        public async Task<ResponseDepartmentDto> AddDepartmentAsync(CreateDepartmentDto department)
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            var dept = _mapper.Map<Department>(department);
            await _context.Departments.AddAsync(dept);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ResponseDepartmentDto>(dept);
            return result;
        }

        // ============================
        // Delete
        // ============================
        public async Task<ResponseDepartmentDto> DeleteDepartmentAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");

            var dept = _context.Departments.Find(id);
            if (dept == null)
                throw new KeyNotFoundException("Department Not Found");

            
            _context.Departments.Remove(dept);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ResponseDepartmentDto>(dept);
            return result;
        }

        // ============================
        // Get All (With Pagination)
        // ============================
        public async Task<IEnumerable<ResponseDepartmentDto>> GetAllDepartmentsAsync(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Invalid pagination parameters.");

            var departments = await _context.Departments
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = _mapper.Map<IEnumerable<ResponseDepartmentDto>>(departments);
            return result;
        }

        // ============================
        // Get By Id
        // ============================
        public async Task<ResponseDepartmentDto> GetDepartmentByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id.");

            var department = await _context.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");

            var result = _mapper.Map<ResponseDepartmentDto>(department);
            return result;
        }

        // ============================
        // Update
        // ============================
        public async Task<ResponseDepartmentDto> UpdateDepartmentAsync(int id, CreateDepartmentDto departmentDto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id.");

            if (departmentDto == null)
                throw new ArgumentNullException(nameof(departmentDto));

            var entity = await _context.Departments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException($"Department with Id {id} not found.");

            // دي أقوى ميزة في AutoMapper 👇
           var response = _mapper.Map(departmentDto, entity);

            await _context.SaveChangesAsync();
            var result = _mapper.Map<ResponseDepartmentDto>(response);

            return result;
        }
    }
}
