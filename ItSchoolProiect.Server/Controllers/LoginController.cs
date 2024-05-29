using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItSchoolProiect.Server.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult PostLogin()
        {
            try
            {
                //Any fails goes to catch
                return Ok();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
