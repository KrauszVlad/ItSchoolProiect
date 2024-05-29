using ItSchoolProiect.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ItSchoolProiect.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> PostLogin(LoginRequest model)

        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user is null)
                {
                    return BadRequest("User Does Not Exist");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var token = GenerateJwtToken(user.Id, user.UserName, roles.ToArray());

                    LoginResponse response = new()
                    {
                        Token = token,
                        Expire = DateTime.UtcNow.AddHours(1)
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid username or password");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private string GenerateJwtToken(string userId, string username, string[] roles)
        {
            var jwtSecretKey = _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecretKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, string.Join(",", roles)),
                }),
                Expires = DateTime.UtcNow.AddMinutes(60), // Token expiration time
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"], // Use the provided issuer
                Audience = _configuration["Jwt:Audience"], // Use the provided audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
