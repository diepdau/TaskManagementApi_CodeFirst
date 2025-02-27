using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Models;
using TaskManagement.Repositories;

namespace TaskManagement.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private CategoryRepository _categoryRepository;
        public CategoryController(CategoryRepository categoryRepository) {
            categoryRepository = _categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAllCategories() => Ok(_categoryRepository.GetAll());

        [HttpPost]
        public IActionResult AddCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name)
                || string.IsNullOrWhiteSpace(category.Description))
                return BadRequest("Name and Des are required");
            if (_categoryRepository.GetByName(category.Name) != null)
                return Conflict("Category name must be  unique");
            _categoryRepository.Add(category);
            return CreatedAtAction(nameof(GetAllCategories), new { id = category.Id }, category);
        }

        [HttpGet("search")]
        public IActionResult SearchPage(string? keyword, int page=1, int pageSize=3)
        {
            int totalItems;

            var tasks = _categoryRepository.GetPaged(
                filter: t => string.IsNullOrEmpty(keyword) || t.Name.Contains(keyword)
                || t.Description.Contains(keyword),
                page: page,
                pageSize: pageSize,
                totalItems: out totalItems);
            return Ok(new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = tasks
            });
        }

    }
}
