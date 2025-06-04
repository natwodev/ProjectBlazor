using AutoMapper;
using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>();
        CreateMap<CreateCategoryDTO, Category>();
        CreateMap<UpdateCategoryDTO, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Bỏ ánh xạ cũ Order->OrderDto tạm thời để tùy chỉnh mới
        CreateMap<Order, OrderDto>()
            // Map OrderDetails (List<OrderDetail>) thành List<ProductDto> lấy từ OrderDetail.Product
            .ForMember(dest => dest.OrderDetails,
                opt => opt.MapFrom(src => src.OrderDetails.Select(od => od.Product)));

        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<OrderDetail, OrderDetailDto>();
        CreateMap<CreateOrderDetailDto, OrderDetail>();
        CreateMap<UpdateOrderDetailDto, OrderDetail>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product.Image));
        CreateMap<CreateCartItemDto, CartItem>();
        CreateMap<UpdateCartItemDto, CartItem>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}