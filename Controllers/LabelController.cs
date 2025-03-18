using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;
using TaskManagementApi.Interfaces;
namespace TaskManagementApi.Controllers
{
    [Route("api/labels")]
    [ApiController]
    //[Authorize]
    public class LabelController : ControllerBase
    {
        private readonly IGenericRepository<Label> _labelRepository;
        private readonly IMapper _mapper;

        public LabelController(IGenericRepository<Label> labelRepository, IMapper mapper)
        {
            _labelRepository = labelRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLabel()
        {
            var labels = _labelRepository.GetAll();
            return Ok(labels);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddLabel([FromBody] LabelDto labelDto)
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
            if ((await _labelRepository.GetAsync(o => o.Name == labelDto.Name)) != null)
                return Conflict("Label name must be unique.");
            var label = _mapper.Map<Label>(labelDto);
            await _labelRepository.Add(label);
            return CreatedAtAction(nameof(AddLabel), new { id = label.Id }, label);
        }
    }
}
