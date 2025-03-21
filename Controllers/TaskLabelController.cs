using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/task-labels")]
    [ApiController]
    //[Authorize]
    public class TaskLabelController : ControllerBase
    {
        private readonly IGenericRepository<TaskLabel> _taskLabelRepository;
        private readonly IGenericRepository<Models.Task> _taskRepository;
        private readonly IGenericRepository<Label> _labelRepository;
        private readonly IMapper _mapper;
        public TaskLabelController(IGenericRepository<TaskLabel> taskLabelRepository, IGenericRepository<Models.Task> taskRepository, IGenericRepository<Label> labelRepository, IMapper mapper)
        {
            _taskLabelRepository = taskLabelRepository;
            _taskRepository = taskRepository;
            _labelRepository = labelRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTaskLabel()
        {
            var tasksLabels = _taskLabelRepository.GetAll();
            return Ok(tasksLabels);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskLabelByTaskId(int id)
        {
            var tasksLabel = await _taskLabelRepository.GetAsync(tl => tl.TaskId == id);
            if (tasksLabel == null)
                return NotFound("Task label not found.");
            return Ok(tasksLabel);
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

            if (await _taskRepository.GetById(taskLabelDto.TaskId) == null)
                return NotFound($"Task with Id {taskLabelDto.TaskId} does not exist.");

            if (await _labelRepository.GetById(taskLabelDto.LabelId) == null)
                return NotFound($"Label with Id {taskLabelDto.LabelId} does not exist.");
            var existingTaskLabel = await _taskLabelRepository.GetAsync(
                       tl => tl.TaskId == taskLabelDto.TaskId && tl.LabelId == taskLabelDto.LabelId);

            if (existingTaskLabel != null)
                return Conflict("This TaskLabel already exists.");

            var taskLabel = _mapper.Map<TaskLabel>(taskLabelDto);
            await _taskLabelRepository.Add(taskLabel);

            return CreatedAtAction(nameof(AddTaskLabel), new { taskLabel.TaskId, taskLabel.LabelId }, taskLabelDto);
        }

        [HttpDelete("{taskId}/{labelId}")]
        public async Task<IActionResult> DeleteTaskLabel(int taskId, int labelId)
        {
            var taskLabel = await _taskLabelRepository.GetAsync(tl => tl.TaskId == taskId && tl.LabelId == labelId);
            if (taskLabel == null)
                return NotFound("TaskLabel not found.");

            await _taskLabelRepository.DeleteT(taskLabel);
            return NoContent();
        }
        

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskLabelsByTaskId(int taskId)
        {
            var task = await _taskRepository.GetById(taskId);
            if (task == null)
            {
                return NotFound($"Task with Id {taskId} does not exist.");
            }

            var taskLabels = await _taskLabelRepository.GetAll()
            .Where(tl => tl.TaskId == taskId)
            .Include(tl => tl.Labels) 
                .ToListAsync(); 

            var result = taskLabels.Select(tl => new
            {
                tl.TaskId,
                tl.LabelId,
                LabelName = tl.Labels != null ? tl.Labels.Name : "Unknown"
            });

            return Ok(result);
        }

    }
}