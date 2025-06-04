using backend_blazor.DTOs;
using backend_blazor.Models;

namespace backend_blazor.Repositories.Interfaces;

public interface ICartItemRepository
{
    Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync();
    Task<CartItemDto?> GetCartItemDtoByIdAsync(int id);
    Task<CartItem?> GetCartItemByIdAsync(int id);
    Task<CartItemDto> CreateAsync(string id, CreateCartItemDto dto);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task<bool> DeleteCartItemAsync(int id);
    Task<bool> DeleteAllCartItemsAsync();
    Task<CartItem?> GetCartItemByProductAsync(int productId);
    Task<CartItem?> GetCartItemByProductAsync(int productId, string userId);
    Task<OrderDto> CheckoutAsync(CreateOrderDto orderDto);
    Task<IEnumerable<CartItemDto>> GetCartItemsByUserIdAsync(string userId);
} 