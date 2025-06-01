using backend_blazor.DTOs;
using backend_blazor.Services.Interfaces;
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
        public async Task<ActionResult<CategoryDTO>> CreateCategory(CreateCategoryDTO dto)
        {
            var result = await _categoryService.CreateAsync(dto);
            return Ok(result);
        }

    
    }
}