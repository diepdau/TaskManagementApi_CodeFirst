using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    [Authorize]
    public class TaskLabelController : ControllerBase
    {
        private readonly TaskLabelRepository _taskLabelRepository;
        private readonly TaskRepository _taskRepository;
        private readonly LabelRepository _labelRepository;
        private readonly IMapper _mapper;
        public TaskLabelController(TaskLabelRepository taskLabelRepository, TaskRepository taskRepository, LabelRepository labelRepository, IMapper mapper)
        {
            _taskLabelRepository = taskLabelRepository;
            _taskRepository = taskRepository;
            _labelRepository = labelRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskLabel([FromBody] TaskLabelDto taskLabelDto)
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

            if (_taskRepository.GetById(taskLabelDto.TaskId) == null)
                return NotFound($"Task with Id {taskLabelDto.TaskId} does not exist.");

            if (_labelRepository.GetById(taskLabelDto.LabelId) == null)
                return NotFound($"Label with Id {taskLabelDto.LabelId} does not exist.");

            if (await _taskLabelRepository.Exists(taskLabelDto.TaskId, taskLabelDto.LabelId))
                return Conflict("TaskLabel already exists.");

            var taskLabel = _mapper.Map<TaskLabel>(taskLabelDto); 
            await _taskLabelRepository.Add(taskLabel);
            return CreatedAtAction(nameof(AddTaskLabel), new { taskLabel.TaskId, taskLabel.LabelId }, taskLabelDto);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveLabel([FromBody] TaskLabelDto taskLabelDto)
        {
            if (!(await _taskLabelRepository.RemoveLabel(taskLabelDto.TaskId, taskLabelDto.LabelId)))
            {
                return NotFound(new { message = $"Label with Id {taskLabelDto.LabelId} is not associated with Task Id {taskLabelDto.TaskId}." });
            }
            return NoContent();
        }
    }
}