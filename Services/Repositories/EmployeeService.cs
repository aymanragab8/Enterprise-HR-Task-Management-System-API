using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dtos.Employee;
using WebApplication2.Mapping;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services.Repositories
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public EmployeeService(IMapper mapper , ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ResponseEmployeeDto> CreateEmployeeAsync(CreateEmployeeDto employee)
        {
            if (employee == null) 
                throw new ArgumentNullException(nameof(employee));

           var emp = _mapper.Map<Employee>(employee);
           await _context.Employees.AddAsync(emp);
           await _context.SaveChangesAsync();

           var response = _mapper.Map<ResponseEmployeeDto>(emp);
           return response;
        }

        public async Task<ResponseEmployeeDto> DeleteEmployeeAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");
            
            var emp = _context.Employees.Find(id);
            if (emp == null)
                throw new KeyNotFoundException("Employee Not Found");
            
            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ResponseEmployeeDto>(emp);
            return result;
        }

        public async Task<IEnumerable<ResponseEmployeeDto>> GetAllEmployeesAsync(int PageNumber = 1, int PageSize = 10)
        {
            if (PageNumber <= 0 || PageSize <= 0)
                throw new ArgumentException("Invalid pagination parameters.");

            
         var employees =  await _context.Employees.Skip((PageNumber - 1) * PageSize)
                                   .AsNoTracking()
                                   .Take(PageSize)
                                   .ToListAsync();

           return _mapper.Map<IEnumerable<ResponseEmployeeDto>>(employees);
        }

        public async Task<ResponseEmployeeDto> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");
            var emp =await _context.Employees.FindAsync(id);
            if (emp == null)
                throw new KeyNotFoundException("Employee Not Found");
            var result = _mapper.Map<ResponseEmployeeDto>(emp);
            return result;
        }

        public async Task<ResponseEmployeeDto> UpdateEmployeeAsync(int id, CreateEmployeeDto employee)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");
            if (employee == null)
                throw new KeyNotFoundException();
            var emp =await _context.Employees.FindAsync(id);
            if (emp == null)
                throw new KeyNotFoundException("Employee Not Found");
           var result = _mapper.Map<ResponseEmployeeDto>(emp);
            return result;
        }
    }
}
