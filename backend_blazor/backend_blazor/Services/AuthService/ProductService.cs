using AutoMapper;
using backend_blazor.DTOs;
using backend_blazor.Repositories.Interfaces;
using backend_blazor.Services.Interfaces;

namespace backend_blazor.Services.AuthService;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository; 
        _mapper = mapper;
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        return await _productRepository.GetProductDtoByIdAsync(id);
    }
    
    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        dto.Image = !string.IsNullOrEmpty(dto.ImageBase64)
            ? Convert.FromBase64String(dto.ImageBase64)
            : null;
          
        return await _productRepository.CreateAsync(dto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteProductAsync(id);
    }
    
    public async Task UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        if (dto.Name != null)
            product.Name = dto.Name;

        if (dto.Description != null)
            product.Description = dto.Description;

        if (dto.Price.HasValue)
            product.Price = dto.Price.Value;

        dto.Image = !string.IsNullOrEmpty(dto.ImageBase64)
            ? Convert.FromBase64String(dto.ImageBase64)
            : dto.Image;

        if (dto.Image != null)
            product.Image = dto.Image;

        if (dto.CategoryId.HasValue)
            product.CategoryId = dto.CategoryId.Value;

        await _productRepository.UpdateProductAsync(product);
    }



    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _productRepository.GetAllProductDtosAsync();
    }

}