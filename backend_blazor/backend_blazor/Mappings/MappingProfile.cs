using AutoMapper;
using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();
            
            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();
        }
    }
}