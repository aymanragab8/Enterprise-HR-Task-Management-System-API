using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication2.Dtos.Auth;
using WebApplication2.Models.Entities;
using WebApplication2.Roles;



namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<ApplicationUser> userManager ,IConfiguration config )
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerdetails)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerdetails.UserName,
                Email = registerdetails.Email
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerdetails.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,"User");
                return Ok("User created successfully");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto logindetails)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(logindetails.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid email address");
                return BadRequest(ModelState);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, logindetails.Password);
            if (!isPasswordValid)
            {
                ModelState.AddModelError("Password", "Invalid password");
                return BadRequest(ModelState);
            }

            //Design JWT token here

            List<Claim> userclaims = new List<Claim>                //5
            {
                // Add claims as needed, for example: Id,UserName, Role, etc.
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };
            var userRoles = await _userManager.GetRolesAsync(user);         //6
            foreach (var role in userRoles)
            {
                userclaims.Add(new Claim(ClaimTypes.Role, role));
            }
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"])); //9

            SigningCredentials signingCred = new SigningCredentials(            //8

                securityKey, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(              //1
                issuer: _config["JWT:IssuerUrl"],                     //2
                audience: _config["JWT:AudienceUrl"],                   //3
                expires : DateTime.Now.AddHours(1),                     //4
                claims : userclaims ,                                   //7
                signingCredentials : signingCred                        //10
                );
            
            // generate the token string and return it to the client
            return Ok(new                                                   //11
        {
            mytoken = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = DateTime.Now.AddHours(1)
        } );
        }

    }
}
