using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models.Entities;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignRoleAdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AssignRoleAdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("{userName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignAdminRole(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound("User not found");
            }
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                return Ok("Admin role assigned successfully");
            }
            else
            {
                return BadRequest("Failed to assign Admin role");
            }
        }
    }
}
