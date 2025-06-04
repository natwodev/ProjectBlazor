using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Repositories.Interfaces;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync();
    Task<CartItemDto?> GetCartItemDtoByIdAsync(int id);
    Task<CartItem?> GetCartItemByIdAsync(int id);
    Task<CartItemDto> CreateAsync(CreateCartItemDto dto);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task<bool> DeleteCartItemAsync(int id);
    Task<bool> DeleteAllCartItemsAsync();
    Task<CartItem?> GetCartItemByProductAsync(int productId);
} 