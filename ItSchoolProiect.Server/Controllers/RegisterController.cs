using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ItSchoolProiect.Server.Models;

namespace ItSchoolProiect.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostRegister(RegisterRequest model)
        {
            try
            {
                IdentityUser newUser = new()
                {
                    UserName = model.Username,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "User");
                    return Ok("Account Created Successfully");
                }
                else 
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
