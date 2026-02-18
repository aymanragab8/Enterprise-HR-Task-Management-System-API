using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebApplication2.Dtos.Auth;
using WebApplication2.Enums;
using WebApplication2.Models.Data;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager,IConfiguration config,IMapper mapper,ITokenService tokenService , ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
        }
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto));

            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already exists");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = _mapper.Map<ApplicationUser>(registerDto);

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                    throw new AggregateException("Not Succeded");

                await _userManager.AddToRoleAsync(user, "Employee");

                var employee = _mapper.Map<Employee>(registerDto);

                employee.ApplicationUserId = user.Id;
                employee.IsManager = false;
                employee.HireDate = DateTime.UtcNow;
                employee.Status = EmployeeStatus.Active;
                employee.IsDeleted = false;
                employee.CreatedAt = DateTime.UtcNow;

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Employee created successfully";
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; 
            }
        }

        public async Task<LoginDetailsDto> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = await _tokenService.GenerateJwtToken(user);

            return new LoginDetailsDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            };
        }


    }
}
