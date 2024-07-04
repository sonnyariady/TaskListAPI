using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TasklistAPI.Interface;
using TasklistAPI.Model.Request;

namespace TasklistAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest input)
        {
            var result = await _userServices.Login(input);

            return Ok(result);

        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(CreateUserRequest input)
        {
            var result = await _userServices.Create(input);

            return Ok(result);

        }

        [HttpPost("CurrentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            //var result = await _userServices.Login(input);
            var email = User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            var userid = User.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value;
            return Ok(userid);

        }
    }
}
