using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TaskManagement.Models;
using TaskManagement.Repositories;
using TaskManagement.DTOs;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace TaskManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private readonly UserRepository _userRepository;
        //private readonly IConfiguration _configuration;
        //public UserController(UserRepository userRepository, IConfiguration configuration)
        //{
        //    _userRepository = userRepository;
        //    _configuration = configuration;
        //}




        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] User user)
        //{
        //    if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
        //    {
        //        return BadRequest("Email or Password cannot be blank.");
        //    }
        //    var userLogin = _userRepository.GetByEmail(user.Email);
        //    if (userLogin == null)
        //    {
        //        return Unauthorized("Email or password is incorrect.");
        //    }
        //    if (!BCrypt.Net.BCrypt.Verify(user.PasswordHash, userLogin.PasswordHash))
        //    {
        //        return Unauthorized("Email or password is incorrect.");
        //    }

        //    var claims = new[]
        //    {   new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
        //        new Claim(ClaimTypes.Name, userLogin.Username),
        //        new Claim(ClaimTypes.Email, userLogin.Email),
        //        new Claim(ClaimTypes.Role, userLogin.Role),
        //    };

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(
        //        _configuration["Jwt:Issuer"],
        //        _configuration["Jwt:Audience"],
        //        claims,
        //        expires: DateTime.UtcNow.AddHours(3),
        //        signingCredentials: credentials);

        //    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        //    return Ok(new
        //    {
        //        Username = userLogin.Username,
        //        Email = userLogin.Email,
        //        Role = userLogin.Role,
        //        Token = accessToken
        //    });
        //}


        //[HttpGet]
        ////[Authorize(Roles = "Admin")]
        //public IActionResult GetAllUsers()
        //{
        //    var users = _userRepository.GetAll();
        //    return Ok(users);
        //}
        //private bool IsValidEmail(string email)
        //{
        //    string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        //    return Regex.IsMatch(email, pattern);
        //}

        //[HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult DeleteUser(int id)
        //{
        //    var user = _userRepository.GetById(id);
        //    if (user == null)
        //    {
        //        return NotFound(new { message = $"User with Id {id} does not exist." });
        //    }

        //    _userRepository.Delete(id);
        //    return NoContent();
        //}
        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;
        //private readonly IConfiguration _configuration;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //public UserController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //}
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(result.Errors);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256));

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized();
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded)
                {
                    return Ok(new { message = "Role added successfully" });
                }

                return BadRequest(result.Errors);
            }

            return BadRequest("Role already exists");
        }
        
    }
}
   
