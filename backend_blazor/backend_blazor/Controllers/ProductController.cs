


using backend_blazor.DTOs;
using backend_blazor.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend_blazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase  
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }
        
            
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct(CreateProductDTO dto)
        {
            var result = await _productService.CreateAsync(dto);
            return Ok(result);
        }
        
        [HttpPatch("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, UpdateProductDTO dto)
        {
            await _productService.UpdateAsync(id, dto);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteAsync(id);
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            return Ok(result);
        }

    }
}

