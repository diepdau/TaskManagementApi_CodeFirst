﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi;
using TaskManagementApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using TaskManagementApi.DTOs;

namespace TaskManagementApi.Controllers
{
    
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(CategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAll();
            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoriesDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name) || string.IsNullOrWhiteSpace(categoryDto.Description))
                return BadRequest("Category name and description are required.");

            if (_categoryRepository.GetByName(categoryDto.Name) != null)
                return Conflict("Category name must be unique.");

            var category = _mapper.Map<Category>(categoryDto);
           await _categoryRepository.Add(category);

            var createdCategoryDto = _mapper.Map<Category>(category);
            return CreatedAtAction(nameof(GetAllCategories), new { id = createdCategoryDto.Id }, createdCategoryDto);
        }

    }
}
