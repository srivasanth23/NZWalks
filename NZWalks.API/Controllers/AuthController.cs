using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepositary _tokenRepo;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepositary tokenRepositary) // UserManager is Provided by Identity
        {
            _userManager = userManager;
            _tokenRepo = tokenRepositary;
        }



        // POST : /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            // If user entered Username or userEmail (then we can share anyone in CreateAsync method)
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };

            // Share Username and password
            var IdentityResult = await _userManager.CreateAsync(identityUser, registerRequestDTO.Password);


            if (IdentityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    IdentityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDTO.Roles);

                    if (IdentityResult.Succeeded)
                    {
                        return Ok("User was registered! Please login.");
                    }
                }
            }
            return BadRequest("Something went Wrong");

        }




        // POST : /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                if (checkPasswordResult)
                {
                    // Get the roles
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create a token
                        // It must be in Repositary method
                        var jwtToken = _tokenRepo.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        return Ok(response);
                    }
                }

            }
            return BadRequest("UserName or Password incorrect");
        }
    }
}
