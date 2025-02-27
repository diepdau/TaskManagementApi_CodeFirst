using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.DTOs;
using TaskManagement.Interfaces;
using TaskManagement.Repositories;

namespace TaskManagement.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    //[Authorize]
    public class TaskController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly CategoryRepository _categoryRepository;

        private readonly IMapper _mapper;
        private readonly TaskRepository _taskRepository;

        public TaskController(IMapper mapper, TaskRepository taskRepository, UserRepository userRepository, CategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            var tasks = _taskRepository.GetAll();
            return Ok(_mapper.Map<IEnumerable<TaskDto>>(tasks));
        }

        [HttpGet("{id}")]
        public ActionResult GetTaskById(int id)
        {
            var task = _taskRepository.GetById(id);
            return task != null ? Ok(task) : NotFound();
        }
        [HttpPost]
        public IActionResult AddTask([FromBody] Models.Task task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Title) || string.IsNullOrWhiteSpace(task.Description) || task.CategoryId == null || task.UserId == null)
                return BadRequest("Requires entering all fields.");

            if (_userRepository.GetById((int)task.UserId) == null)
                return NotFound($"User with Id {task.UserId} does not exist.");

            if (_categoryRepository.GetById((int)task.CategoryId) == null)
                return NotFound($"Category with Id {task.CategoryId} does not exist.");

            var newTask = new Models.Task
            {
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted ?? false,
                UserId = task.UserId,
                CategoryId = task.CategoryId,
                CreatedAt = task.CreatedAt ?? DateTime.UtcNow
            };

            _taskRepository.Add(newTask);
            return CreatedAtAction(nameof(GetTaskById), new { id = newTask.Id }, newTask);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] Models.Task task)
        {
            var existingTask = _taskRepository.GetById(id);
            if (existingTask == null)
                return NotFound(new { message = "Task not found." });
            if (task.CategoryId != null)
            {
                if (_categoryRepository.GetById((int)task.CategoryId) == null)
                    return NotFound($"Category with Id {task.CategoryId} does not exist.");
                existingTask.CategoryId = task.CategoryId;
            }
            else if (task.UserId != null)
            {
                if (_userRepository.GetById((int)task.UserId) == null)
                    return NotFound($"User with Id {task.UserId} does not exist.");
                existingTask.UserId = task.UserId;
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description != null ? task.Description : existingTask.Description;
            existingTask.IsCompleted = task.IsCompleted != null ? task.IsCompleted : existingTask.IsCompleted;
            existingTask.CreatedAt = task.CreatedAt ?? DateTime.UtcNow;

            _taskRepository.Update(existingTask);
            return Ok(existingTask);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var taskDel = _taskRepository.GetById(id);
            if (taskDel == null)
            {
                return NotFound(new { message = $"task with Id {id} does not exist." });
            }

            _taskRepository.Delete(id);
            return NoContent();
        }


    }
}
