using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Repositories.Interfaces;

public interface IProductRepository
{
    Task UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<ProductDTO?> GetProductDtoByIdAsync(int id);
    Task<ProductDTO> CreateAsync(CreateProductDTO dto);
    Task<IEnumerable<ProductDTO>> GetAllProductDtosAsync();
    Task<Product?> GetProductByIdAsync(int id);
}