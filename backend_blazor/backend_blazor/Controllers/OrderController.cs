using System.Security.Claims;
using backend_blazor.DTOs;
using backend_blazor.Repositories.Interfaces;
using backend_blazor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_blazor.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IOrderService _orderService ;

    public OrderController(ICartItemRepository cartItemRepository, IOrderService orderService)
    {
        _cartItemRepository = cartItemRepository;
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    [Authorize(Policy = "CustomerOrAdmin")]
    public async Task<ActionResult<OrderDto>> Checkout([FromBody] CreateOrderDto orderDto)
    {
        try
        {
            // Get current user's ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Token không hợp lệ!" });

            // Set the UserId from the authenticated user
            orderDto.UserId = userId;

            var order = await _cartItemRepository.CheckoutAsync(orderDto);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your order");
        }
    }
    [HttpGet("my-orders")]
    [Authorize(Policy = "CustomerOrAdmin")]
    public async Task<ActionResult<OrderDto>> GetOrdersByUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Token không hợp lệ!" });

        var order = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(order);
    }
    
    [HttpGet("all-orders")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<OrderDto>> GetAllOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Token không hợp lệ!" });

        var order = await _orderService.GetAllOrdersAsync();
        return Ok(order);
    }
} 