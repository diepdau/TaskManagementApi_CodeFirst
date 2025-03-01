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
        public IActionResult AddTaskLabel([FromBody] TaskLabelDto taskLabelDto)
        {
            if (taskLabelDto?.TaskId <= 0 || taskLabelDto?.LabelId <= 0)
                return BadRequest("Invalid TaskId or LabelId.");

            if (_taskRepository.GetById(taskLabelDto.TaskId) == null)
                return NotFound($"Task with Id {taskLabelDto.TaskId} does not exist.");

            if (_labelRepository.GetById(taskLabelDto.LabelId) == null)
                return NotFound($"Label with Id {taskLabelDto.LabelId} does not exist.");

            if (_taskLabelRepository.Exists(taskLabelDto.TaskId, taskLabelDto.LabelId))
                return Conflict("TaskLabel already exists.");

            var taskLabel = _mapper.Map<TaskLabel>(taskLabelDto); 
            _taskLabelRepository.Add(taskLabel);
            return CreatedAtAction(nameof(AddTaskLabel), new { taskLabel.TaskId, taskLabel.LabelId }, taskLabelDto);
        }

        [HttpDelete]
        public IActionResult RemoveLabel([FromBody] TaskLabelDto taskLabelDto)
        {
            if (!_taskLabelRepository.RemoveLabel(taskLabelDto.TaskId, taskLabelDto.LabelId))
            {
                return NotFound(new { message = $"Label with Id {taskLabelDto.LabelId} is not associated with Task Id {taskLabelDto.TaskId}." });
            }
            return NoContent();
        }
    }
}