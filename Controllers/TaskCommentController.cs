using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-comments")]
    [ApiController]
    [Authorize]
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
            var comment =await _taskCommentRepository.GetById(id);
            if (comment == null)
            {
                return NotFound(new { message = $"Comment with Id {id} does not exist." });
            }

            await _taskCommentRepository.Delete(id);
            return NoContent();
        }
    }
}

