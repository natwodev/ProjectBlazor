using backend_blazor.DTOs;
using backend_blazor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase   
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategorytions()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }
        
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CreateCategoryDTO dto)
        {
            var result = await _categoryService.CreateAsync(dto);
            return Ok(result);
        }
        
        [HttpPatch("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, UpdateCategoryDTO dto)
        {
            await _categoryService.UpdateAsync(id, dto);
            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<bool>> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(result);
        }
        
    }
}