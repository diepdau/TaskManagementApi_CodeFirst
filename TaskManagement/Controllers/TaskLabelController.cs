using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    [Authorize]
    public class TaskLabelController : ControllerBase
    {
        private readonly TaskLabelRepository _taskLabelRepository;
        private readonly TaskRepository _taskRepository;
        private readonly LabelRepository _labelRepository;

        public TaskLabelController(TaskLabelRepository taskLabelRepository, TaskRepository taskRepository, LabelRepository labelRepository)
        {
            _taskLabelRepository = taskLabelRepository;
            _taskRepository = taskRepository;
            _labelRepository = labelRepository;
        }

        [HttpGet]
        public IActionResult GetAllTaskLabels()
        {
            var taskLabels = _taskLabelRepository.GetAll();
            return Ok(taskLabels);
        }


        [HttpPost]
        public IActionResult AddTaskLabel([FromBody] TaskLabel taskLabel)
        {
            if (taskLabel?.TaskId <= 0 || taskLabel?.LabelId <= 0)
                return BadRequest("Invalid TaskId or LabelId.");

            if (_taskRepository.GetById(taskLabel.TaskId) == null)
                return NotFound($"Task with Id {taskLabel.TaskId} does not exist.");

            if (_labelRepository.GetById(taskLabel.LabelId) == null)
                return NotFound($"Label with Id {taskLabel.LabelId} does not exist.");

            if (_taskLabelRepository.Exists(taskLabel.TaskId, taskLabel.LabelId))
                return Conflict("TaskLabel already exists.");

            _taskLabelRepository.Add(taskLabel);
            return CreatedAtAction(nameof(AddTaskLabel), new { taskLabel.TaskId, taskLabel.LabelId }, taskLabel);
        }

        [HttpDelete("{taskId}/{labelId}")]
        public IActionResult RemoveLabel(int taskId, int labelId)
        {
            if (!_taskLabelRepository.RemoveLabel(taskId, labelId))
            {
                return NotFound(new { message = $"Label with Id {labelId} is not associated with Task Id {taskId}." });
            }
            return NoContent();
        }

    }
}