using backend_blazor.DTOs;

namespace backend_blazor.Services.Interfaces;

public interface ICartItemService
{
    Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync();
    Task<CartItemDto?> GetCartItemByIdAsync(int id);
    Task<CartItemDto> AddToCartAsync(string id, CreateCartItemDto dto);
    Task<CartItemDto> UpdateCartItemQuantityAsync(int id, UpdateCartItemDto dto);
    Task<bool> RemoveFromCartAsync(int id);
    Task<IEnumerable<CartItemDto>> GetCartItemsByUserIdAsync(string userId);
} 