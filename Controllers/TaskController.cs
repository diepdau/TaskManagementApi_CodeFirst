using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementApi.DTOs.TaskDto;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]

    public class TaskController : ControllerBase
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public TaskController(TaskRepository taskService, UserRepository userRepository, CategoryRepository categoryRepository,IMapper mapper)
        {
            _taskRepository = taskService;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasks()=> Ok(await _taskRepository.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }
        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskCreateDto taskDto)
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

            if (_userRepository.GetById((int)taskDto.UserId) == null)
                return NotFound($"User with Id {taskDto.UserId} does not exist.");

            if (_categoryRepository.GetById((int)taskDto.CategoryId) == null)
                return NotFound($"Category with Id {taskDto.CategoryId} does not exist.");

            var task = _mapper.Map<Models.Task>(taskDto);
            await _taskRepository.Add(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>  UpdateTask(int id, [FromBody] TaskUpdateDto taskDto)
        {
            var existingTask = await _taskRepository.GetById(id);
            if (existingTask == null)
                return NotFound();
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }
            var currentUserId = int.Parse(userIdClaim);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (existingTask.UserId != currentUserId && userRole != "Admin")
            {
                return StatusCode(403, new { message = "You are not authorized to update this task." });
            }

            if (taskDto.Title != null)
                existingTask.Title = taskDto.Title;

            if (taskDto.Description != null)
                existingTask.Description = taskDto.Description;

            if (taskDto.IsCompleted != null)
                existingTask.IsCompleted = taskDto.IsCompleted;

            if (taskDto.CreatedAt != null)
                existingTask.CreatedAt = taskDto.CreatedAt;

            if (taskDto.CategoryId != null)
            {
                if (_categoryRepository.GetById((int)taskDto.CategoryId) == null)
                    return NotFound(new { message = $"Category with Id {taskDto.CategoryId} does not exist." });
                existingTask.CategoryId = taskDto.CategoryId;
            }
            _mapper.Map(taskDto, existingTask);
            await _taskRepository.Update(existingTask);
            var updatedTaskDto = _mapper.Map<TaskUpdateDto>(existingTask);
            return Ok(new { message = "Task updated successfully.", updatedTask = updatedTaskDto });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepository.GetById(id);
            if (task == null)
                return NotFound(new { message = $"Task with Id {id} does not exist." });

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
            var currentUserId = int.Parse(userIdClaim);
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (task.UserId != currentUserId && userRole != "Admin")
            {
                return StatusCode(403, new { message = "You are not authorized to delete this task." });
            }

           await _taskRepository.Delete(id);

            return Ok(new { message = "Task deleted successfully.", taskId = id });
        }


    }
}