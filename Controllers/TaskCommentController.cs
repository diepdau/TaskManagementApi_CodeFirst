using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-comments")]
    [ApiController]
    //[Authorize]
    public class TaskCommentController : ControllerBase
    {
        private readonly IGenericRepository<TaskComment> _taskCommentRepository;
        private readonly IGenericRepository<Models.Task> _taskRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper; 

        public TaskCommentController(IGenericRepository<TaskComment> taskCommentRepository, IGenericRepository<User> userRepository, IGenericRepository<Models.Task> taskRepository, IMapper mapper)
        {
            _taskCommentRepository = taskCommentRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTaskComment()
        {
            var tasksComment = _taskCommentRepository.GetAll();
            return Ok(tasksComment);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskCommentByTaskId(int id)
        {
            var tasksComment = await _taskCommentRepository.GetAsync(tl => tl.TaskId == id);
            if (tasksComment == null)
                return NotFound("Task comment not found.");
            return Ok(tasksComment);
        }
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] TaskCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _taskRepository.GetById(commentDto.TaskId) == null)
                return NotFound($"Task with Id {commentDto.TaskId} does not exist.");

            if (await   _userRepository.GetById(commentDto.UserId) == null)
                return NotFound($"User with Id {commentDto.UserId} does not exist.");

            var comment = _mapper.Map<TaskComment>(commentDto); 
            comment.CreatedAt = DateTime.UtcNow;
            await _taskCommentRepository.Add(comment);
            return CreatedAtAction(nameof(AddComment), new { id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { message = $"Comment with Id {id} does not exist." });
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            var currentUserId = int.Parse(userIdClaim);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (comment.UserId != currentUserId && userRole != "Admin")
            {
                return StatusCode(403, new { message = "You are not authorized to delete this task comment." });
            }

            await _taskCommentRepository.Delete(id);
            return NoContent();
        }
        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskCommentsByTaskId(int taskId)
        {
            var task = await _taskRepository.GetById(taskId);
            if (task == null)
            {
                return NotFound($"Task with Id {taskId} does not exist.");
            }

            var comments = await _taskCommentRepository.GetAll()
            .Where(c => c.TaskId == taskId)
            .Include(c => c.User)
                .ToListAsync(); 

            var result = comments.Select(c => new
            {
                c.Id,
                c.TaskId,
                c.UserId,
                UserName = c.User != null ? c.User.UserName : "Unknown",
                c.Content,
                c.CreatedAt
            });

            return Ok(result);
        }
    }
}

