using Hospital_Management.DTO;
using Hospital_Management.AccountService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Hospital_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;


        public AuthController(IAuthService _authService)
        {
            authService = _authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AuthModel result = await authService.Register(model);
            if(!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(new {token = result.Token , expiresOn = result.ExpiresOn,UserRole = result.Roles});
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(TokenRequestModel model)
        {

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AuthModel result = await authService.Login(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }
            return Ok(new { token = result.Token, expiresOn = result.ExpiresOn, UserRole = result.Roles ,Email = result.Email });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(AddRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string result = await authService.AddRole(model);
            if (!string.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(model);
        }
      
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return NotFound();

            AuthModel result = await authService.ForgetPassword(email);

            if (result.IsSuccess)
            {
                return Ok(result.Token);
            }

            return BadRequest(result.Message); 
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await authService.ResetPassword(model);

                if (result.IsSuccess)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }

            return BadRequest("Some properties are not valid");
        }

    }
}
