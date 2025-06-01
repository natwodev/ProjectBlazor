using backend_blazor.DTOs;

namespace backend_blazor.Services.Interfaces;

public interface IProductService
{
    Task<ProductDTO?> GetByIdAsync(int id);
    Task<ProductDTO> CreateAsync(CreateProductDTO dto);
    Task<bool> DeleteAsync(int id);
    Task UpdateAsync(int id, UpdateProductDTO dto);
    Task<IEnumerable<ProductDTO>> GetAllAsync();
}