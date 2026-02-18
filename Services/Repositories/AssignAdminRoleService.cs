using Microsoft.AspNetCore.Identity;
using WebApplication2.Models.Entities;
using WebApplication2.Services.Interfaces;

namespace WebApplication2.Services.Repositories
{
    public class AssignAdminRoleService : IAssignAdminRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignAdminRoleService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> AssignAdminRole(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new ArgumentException("Not Found");

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
                return ("Admin role assigned successfully");

            return ("Failed to assign Admin role");
        }
    }
}
