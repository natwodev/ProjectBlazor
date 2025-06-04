using AutoMapper;
using backend_blazor.DTOs;
using backend_blazor.Repositories.Interfaces;
using backend_blazor.Services.Interfaces;

namespace backend_blazor.Services.AuthService;

public class CartItemService : ICartItemService
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IMapper _mapper;

    public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper)
    {
        _cartItemRepository = cartItemRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync()
    {
        return await _cartItemRepository.GetAllCartItemsAsync();
    }

    public async Task<CartItemDto?> GetCartItemByIdAsync(int id)
    {
        return await _cartItemRepository.GetCartItemDtoByIdAsync(id);
    }

    public async Task<CartItemDto> AddToCartAsync(CreateCartItemDto dto)
    {
        // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa
        var existingCartItem = await _cartItemRepository.GetCartItemByProductAsync(dto.ProductId);
        
        if (existingCartItem != null)
        {
            // Nếu đã có, cập nhật số lượng
            existingCartItem.Quantity += dto.Quantity;
            await _cartItemRepository.UpdateCartItemAsync(existingCartItem);
            return await _cartItemRepository.GetCartItemDtoByIdAsync(existingCartItem.Id) 
                ?? throw new Exception("Failed to update cart item");
        }

        // Nếu chưa có, tạo mới
        return await _cartItemRepository.CreateAsync(dto);
    }

    public async Task<CartItemDto> UpdateCartItemQuantityAsync(int id, UpdateCartItemDto dto)
    {
        var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
        if (cartItem == null)
            throw new ArgumentException("Cart item not found");

        if (dto.Quantity.HasValue)
        {
            if (dto.Quantity.Value <= 0)
                throw new ArgumentException("Quantity must be greater than 0");

            cartItem.Quantity = dto.Quantity.Value;
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        return await _cartItemRepository.GetCartItemDtoByIdAsync(id) 
            ?? throw new Exception("Failed to update cart item");
    }

    public async Task<bool> RemoveFromCartAsync(int id)
    {
        return await _cartItemRepository.DeleteCartItemAsync(id);
    }

    public async Task<bool> ClearCartAsync()
    {
        return await _cartItemRepository.DeleteAllCartItemsAsync();
    }

    public async Task<CartItemDto?> GetCartItemByProductAsync(int productId)
    {
        var cartItem = await _cartItemRepository.GetCartItemByProductAsync(productId);
        return cartItem != null ? _mapper.Map<CartItemDto>(cartItem) : null;
    }
} 