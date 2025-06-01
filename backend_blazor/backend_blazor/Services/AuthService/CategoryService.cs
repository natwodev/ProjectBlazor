using backend_blazor.DTOs;
using backend_blazor.Models;
using backend_blazor.Repositories.Interfaces;
using backend_blazor.Services.Interfaces;
using AutoMapper;

namespace backend_blazor.Services.AuthService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryDtoByIdAsync(id);
        }

        public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto)
        {
            return await _categoryRepository.CreateAsync(dto);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateCategoryDTO dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            if (dto.Name != null)
                category.Name = dto.Name;

            if (dto.Description != null)
                category.Description = dto.Description;

            await _categoryRepository.UpdateCategoryAsync(category);
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            return await _categoryRepository.GetAllCategoryDtosAsync();
        }

    }
}