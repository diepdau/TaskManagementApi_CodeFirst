using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-comments")]
    [ApiController]
    [Authorize]
    public class TaskCommentController : ControllerBase
    {
        private readonly TaskCommentRepository _taskCommentRepository;
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper; 

        public TaskCommentController(TaskCommentRepository taskCommentRepository, UserRepository userRepository, TaskRepository taskRepository, IMapper mapper)
        {
            _taskCommentRepository = taskCommentRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddComment([FromBody] TaskCommentDto commentDto)
        {
            if (commentDto == null || string.IsNullOrWhiteSpace(commentDto.Content))
                return BadRequest("Comment content is required.");

            if (_taskRepository.GetById(commentDto.TaskId) == null)
                return NotFound($"Task with Id {commentDto.TaskId} does not exist.");

            if (_userRepository.GetById(commentDto.UserId) == null)
                return NotFound($"User with Id {commentDto.UserId} does not exist.");

            var comment = _mapper.Map<TaskComment>(commentDto); 
            comment.CreatedAt = DateTime.UtcNow;
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

