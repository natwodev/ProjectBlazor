using backend_blazor.DTOs;

namespace backend_blazor.Services.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, UpdateProductDto dto);
    Task<IEnumerable<ProductDto>> GetAllAsync();
}