using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApi.Repositories;
using TaskManagementApi.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TaskManagementApi.DTOs;
namespace TaskManagementApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;  

        public UserController(UserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
       
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAll();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);  
            return Ok(userDtos);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user== null)
            {
                return NotFound(new { message = $"User with Id {id} does not exist." });
            }

            _userRepository.Delete(id);
            return NoContent();
        }

    }
}
