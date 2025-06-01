using AutoMapper;
using backend_blazor.Data;
using backend_blazor.DTOs;
using backend_blazor.Models;
using backend_blazor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_blazor.Repositories.AuthRepository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CategoryRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CategoryDTO>> GetAllCategoryDtosAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }

    public async Task UpdateCategoryAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<CategoryDTO?> GetCategoryDtoByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category != null ? _mapper.Map<CategoryDTO>(category) : null;
    }

    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category != null ? _mapper.Map<Category>(category) : null;
    }
    public async Task<CategoryDTO> CreateAsync(CreateCategoryDTO dto)
    {
        var entity = _mapper.Map<Category>(dto);
        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<CategoryDTO>(entity);
    }
    
}