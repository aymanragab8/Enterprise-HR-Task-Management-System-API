using WebApplication2.Models.Entities;

namespace WebApplication2.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(ApplicationUser user);

    }
}
