using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi;
using TaskManagementApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using TaskManagementApi.DTOs;
using TaskManagementApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementApi.Controllers
{
    
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories =  _categoryRepository.GetAll();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoriesDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
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
            if ((await _categoryRepository.GetAsync(o => o.Name == categoryDto.Name))!=null)
                return Conflict("Category name must be unique.");

            var category = _mapper.Map<Category>(categoryDto);
           await _categoryRepository.Add(category);

            var createdCategoryDto = _mapper.Map<Category>(category);
            return CreatedAtAction(nameof(GetAllCategories), new { id = createdCategoryDto.Id }, createdCategoryDto);
        }

    }
}
