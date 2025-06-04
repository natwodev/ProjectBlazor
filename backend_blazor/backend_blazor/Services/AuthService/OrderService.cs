using backend_blazor.DTOs;
using backend_blazor.Repositories.Interfaces;
using backend_blazor.Services.Interfaces;

namespace backend_blazor.Services.AuthService;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }
    
}