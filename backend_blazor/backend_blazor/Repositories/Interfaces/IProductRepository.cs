using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Repositories.Interfaces;

public interface IProductRepository
{
    Task UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<ProductDto?> GetProductDtoByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<IEnumerable<ProductDto>> GetAllProductDtosAsync();
    Task<Product?> GetProductByIdAsync(int id);
}