using WebApplication2.Dtos.Auth;

namespace WebApplication2.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<LoginDetailsDto> LoginAsync(LoginDto loginDto);
    }
}
