using backend_blazor.DTOs;
using backend_blazor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_blazor.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems()
    {
        var cartItems = await _cartItemService.GetAllCartItemsAsync();
        return Ok(cartItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartItemDto>> GetCartItem(int id)
    {
        var cartItem = await _cartItemService.GetCartItemByIdAsync(id);
        if (cartItem == null)
            return NotFound();

        return Ok(cartItem);
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> AddToCart(CreateCartItemDto dto)
    {
        try
        {
            var cartItem = await _cartItemService.AddToCartAsync(dto);
            return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.Id }, cartItem);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CartItemDto>> UpdateQuantity(int id, UpdateCartItemDto dto)
    {
        try
        {
            var cartItem = await _cartItemService.UpdateCartItemQuantityAsync(id, dto);
            return Ok(cartItem);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var result = await _cartItemService.RemoveFromCartAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

  
} 