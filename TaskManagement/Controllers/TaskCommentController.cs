using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Controllers
{

    [Authorize]
    [Route("api/task-comments")]
    [ApiController]
    public class TaskCommentController : ControllerBase
    {
        private readonly TaskCommentRepository _taskCommentRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        public TaskCommentController(TaskCommentRepository taskCommentRepository, UserRepository userRepository, TaskRepository taskRepository)
        {
            _taskCommentRepository = taskCommentRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult AddComment([FromBody] TaskComment comment)
        {
            if (comment == null || string.IsNullOrWhiteSpace(comment.Content))
                return BadRequest("Comment content is required.");

            if (_taskRepository.GetById(comment.TaskId) == null)
                return NotFound($"Task with Id {comment.TaskId} does not exist.");

            if (_userRepository.GetById((int)comment.UserId) == null)
                return NotFound($"User with Id {comment.UserId} does not exist.");

            comment.CreatedAt = comment.CreatedAt ?? DateTime.UtcNow;

            _taskCommentRepository.Add(comment);
            return CreatedAtAction(nameof(AddComment), new { id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteComment(int id)
        {
            var comment = _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { message = $"Comment with Id {id} does not exist." });
            }

            _taskCommentRepository.Delete(id);
            return NoContent();
        }
    }
}
