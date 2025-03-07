using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementApi.Models;
using TaskManagementApi.DTOs;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Azure;
using Microsoft.AspNetCore.Authorization;

namespace TaskManagementApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();

                return BadRequest(new { Errors = errors });
            }
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return StatusCode(500, "User creation failed unexpectedly.");
            var roleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok(new { message = "User registered successfully",
                User = new
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new
                    {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    }).ToList();

                return BadRequest(new { Errors = errors });
            }

            var appUser = await _userManager.FindByEmailAsync(model.Email);
            if (appUser == null)
            {
                return Unauthorized("Invalid username or password");
            }
            var result = await _signInManager.PasswordSignInAsync(appUser.UserName, model.Password, false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid username or password");
            }
            var roles = await _userManager.GetRolesAsync(appUser);
            var token = await GenerateJwtTokenLogin(appUser);

            return Ok(new
            {
                Token = token,
                User = new
                {
                    Id = appUser.Id,
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Roles = roles
                }
            });
        }

        private async Task<string> GenerateJwtTokenLogin(User user)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var token = _tokenHandler.CreateEncodedJwt(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                subject: principal.Identity as ClaimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(3),
                issuedAt: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256)
            );
            return token;
        }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }

    }
}

