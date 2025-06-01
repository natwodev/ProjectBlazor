using backend_blazor.DTOs;

namespace backend_blazor.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto);
        Task<bool> DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateCategoryDTO dto);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();

    }
}