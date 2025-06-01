using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int id);
    Task<CategoryDTO?> GetCategoryDtoByIdAsync(int id);
    Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto);
    Task<IEnumerable<CategoryDTO>> GetAllCategoryDtosAsync();
}