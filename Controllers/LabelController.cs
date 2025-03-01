using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs;
using TaskManagementApi.Models;
using TaskManagementApi.Repositories;

namespace TaskManagementApi.Controllers
{
    [Route("api/labels")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {
        private readonly LabelRepository _labelRepository;
        private readonly IMapper _mapper;

        public LabelController(LabelRepository labelRepository, IMapper mapper)
        {
            _labelRepository = labelRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllLabel()
        {
            var labels = _labelRepository.GetAll(); 
            return Ok(labels);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddLabel([FromBody] LabelDto labelDto)
        {
            if (labelDto == null || string.IsNullOrWhiteSpace(labelDto.Name))
                return BadRequest("Label name is required.");

            if (_labelRepository.GetByName(labelDto.Name) != null)
                return Conflict("Label name must be unique.");
            var label = _mapper.Map<Label>(labelDto);
            _labelRepository.Add(label);
            return CreatedAtAction(nameof(AddLabel), new { id = label.Id }, label);
        }
    }
}
