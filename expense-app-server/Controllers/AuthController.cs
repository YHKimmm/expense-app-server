using expense_app_server.CustomException;
using expense_app_server.Interfaces;
using expense_app_server.Models;
using expense_app_server.Repository;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace expense_app_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(User user)
        {
            try
            {
                var result = await _userRepository.SignUp(user);
                return Created("", result);
            }
            catch (Exception e)
            {
                return StatusCode(409, e.Message);
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(User user)
        {
            try
            {
                var result = await _userRepository.SignIn(user);
                return Ok(result);
            }
            catch(Exception e)
            {
                return StatusCode(401, e.Message);
            }
        }

        [HttpPost("google")]
        public async Task<ActionResult> GoogleSignIn([FromQuery] string token)
        {
           
            var payload = await ValidateAsync(token, new ValidationSettings
            {
                Audience = new[]
                {
                    Environment.GetEnvironmentVariable("CLIENT_ID")
                }
            });

            var result = await _userRepository.ExternalSignIn(new User
            {
                Email = payload.Email,
                ExternalId = payload.Subject,
                ExternalType = "GOOGLE"
            });

            return Created("", result);

        }
    }
}
