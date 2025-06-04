using backend_blazor.DTOs;

namespace backend_blazor.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
}