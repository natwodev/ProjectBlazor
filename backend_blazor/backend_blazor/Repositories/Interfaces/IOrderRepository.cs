using backend_blazor.DTOs;

namespace backend_blazor.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
}