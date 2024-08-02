using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BetDND.Models;
using BetDND.Enums;
using BetDND.Data;
using BetDND.Services;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace BetDND.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly DataContext _context;
        private readonly RegisterService registerService;
        private readonly LoginService loginService;
        private readonly TokenService tokenService;

        public AuthController(DataContext context)
        {
            _context = context;
            registerService = new RegisterService(_context);
            loginService = new LoginService(_context);
            tokenService = new TokenService();
        }

        [HttpPost("login")]
        public IActionResult Login ([FromBody] AuthenticationInput authenticationInput)
        {
            try {
                string token = loginService.AuthenticateUser(authenticationInput);
                return Ok(token);
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("checktoken")]
        [Authorize]
        public IActionResult CheckToken ()
        {
            try {
                string token = tokenService.GetToken(Request.Headers["Authorization"]);
                loginService.IsCurrentUserTokenValid(token);
                return Ok(true);
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }
   
        [HttpPost("register")]
        public IActionResult Register ([FromBody] RegistrationInput registrationInput)
        {
            try {
                string nameSurname = registrationInput.NameSurname;
                AuthenticationInput authenticationInput = new AuthenticationInput {
                    Email = registrationInput.Email,
                    Password = registrationInput.Password
                };
                User user = registerService.CreateUser(authenticationInput, nameSurname);
                return Ok(user.Email);
            } catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }
        
    }
}
