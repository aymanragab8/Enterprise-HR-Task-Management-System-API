using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Dtos.Employee;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services.Repositories
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeService(IMapper mapper , ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }
        public async Task<EmployeeDetailsDto> AddEmployeeAsync(CreateEmployeeRequestDto request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var user = new ApplicationUser { UserName = request.EmployeeData.Email, Email = request.EmployeeData.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            string role =  "Employee" ;
            await _userManager.AddToRoleAsync(user, role);

            var employee = _mapper.Map<Employee>(request.EmployeeData);
            employee.ApplicationUserId = user.Id;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeDetailsDto>(employee);
        }


        public async Task<EmployeeDetailsDto> DeleteEmployeeAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");
            
            var emp =await _context.Employees.FindAsync(id);
            if (emp == null)
                throw new KeyNotFoundException("Employee Not Found");
            
            emp.IsDeleted=true;
            await _context.SaveChangesAsync();

            var result = _mapper.Map<EmployeeDetailsDto>(emp);
            return result;
        }

        public async Task<IEnumerable<AllEmployeesDto>?> GetEmployeesAsync(
            string currentUserId,
            string role,
            int? departmentId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            
            var currentEmployee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);

            if (currentEmployee == null)
                return null;

            var query = _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            if (role == "Employee")
            {
                if (departmentId.HasValue)
                    return null; 

                query = query.Where(e => e.Id == currentEmployee.Id);
            }

            else if (role == "Manager")
            {
                if (!departmentId.HasValue)
                {
                    query = query.Where(e => e.DepartmentId == currentEmployee.DepartmentId);
                }
                else
                {
                    if (departmentId.Value != currentEmployee.DepartmentId)
                        return null; 

                    query = query.Where(e => e.DepartmentId == departmentId.Value);
                }
            }


            query = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var employees = await query.ToListAsync();

            return _mapper.Map<IEnumerable<AllEmployeesDto>>(employees);
        }



        public async Task<EmployeeDetailsDto> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");
            var emp =await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (emp == null)
                throw new KeyNotFoundException("Employee Not Found");
            var result = _mapper.Map<EmployeeDetailsDto>(emp);
            return result;
        }

        public async Task<EmployeeDetailsDto> UpdateEmployeeAsync(int id, UpdateEmployeeDto employee, string currentUserId, string role)
        {
            if (id <= 0)
                throw new ArgumentException("Enter Valid Id");
            if (employee == null)
                throw new KeyNotFoundException();
            var emp =await _context.Employees.FindAsync(id);
            if (emp == null)
                throw new KeyNotFoundException("Employee Not Found");
            if (role == "Manager")
            {
                var manager =await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);
                if (manager.DepartmentId != emp.DepartmentId)
                    throw new ArgumentException("Can't Update Data ");
                _mapper.Map(employee, emp);
            }
            else if (role == "Employee")
            {
                var employeerole = await _context.Employees.FirstOrDefaultAsync(e => e.ApplicationUserId == currentUserId);
                if (employeerole.Id != id)
                    throw new ArgumentException("Can't Update Data ");

                emp.PhoneNumber = employee.PhoneNumber;
                emp.Address = employee.Address;
                emp.JobTitle = employee.JobTitle;
            }
            else _mapper.Map(employee, emp);

            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeDetailsDto>(emp);
        }
    }
}
