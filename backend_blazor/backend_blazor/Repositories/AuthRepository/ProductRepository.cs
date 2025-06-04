using AutoMapper;
using backend_blazor.Data;
using backend_blazor.DTOs;
using backend_blazor.Models;
using backend_blazor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_blazor.Repositories.AuthRepository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ProductDto>> GetAllProductDtosAsync()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    
    public async Task UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<ProductDto?> GetProductDtoByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product != null ? _mapper.Map<Product>(product) : null;
    }
    
    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var entity = _mapper.Map<Product>(dto);
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProductDto>(entity);
    }
    
    
}