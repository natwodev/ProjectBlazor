using AutoMapper;
using backend_blazor.Data;
using backend_blazor.DTOs;
using backend_blazor.Models;
using backend_blazor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend_blazor.Repositories.AuthRepository;

public class CartItemRepository : ICartItemRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CartItemRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CartItemDto>> GetCartItemsByUserIdAsync(string userId)
    {
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
    }

    public async Task<IEnumerable<CartItemDto>> GetAllCartItemsAsync()
    {
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
    }

    public async Task<CartItemDto?> GetCartItemDtoByIdAsync(int id)
    {
        var cartItem = await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == id);

        return cartItem != null ? _mapper.Map<CartItemDto>(cartItem) : null;
    }

    public async Task<CartItem?> GetCartItemByIdAsync(int id)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<CartItemDto> CreateAsync(string id,CreateCartItemDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            throw new ArgumentException("Product not found");

        var cartItem = _mapper.Map<CartItem>(dto);
        cartItem.Price = product.Price;
        cartItem.UserId = id; 

        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        return await GetCartItemDtoByIdAsync(cartItem.Id) 
            ?? throw new Exception("Failed to create cart item");
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteCartItemAsync(int id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
            return false;

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllCartItemsAsync()
    {
        var cartItems = await _context.CartItems.ToListAsync();
        if (!cartItems.Any())
            return false;

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<CartItem?> GetCartItemByProductAsync(int productId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.ProductId == productId);
    }

    public async Task<CartItem?> GetCartItemByProductAsync(int productId, string userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);
    }

    public async Task<OrderDto> CheckoutAsync(CreateOrderDto orderDto)
    {
        // Get all cart items
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .ToListAsync();

        if (!cartItems.Any())
            throw new InvalidOperationException("Cart is empty");

        // Create new order
        var order = new Order
        {
            Name = orderDto.Name,
            Phone = orderDto.Phone,
            Address = orderDto.Address,
            UserId = orderDto.UserId,
            CreatedAt = DateTime.Now,
            OrderDetails = cartItems.Select(item => new OrderDetail
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList()
        };

        // Add order to database
        _context.Orders.Add(order);
        
        // Clear cart items
        _context.CartItems.RemoveRange(cartItems);
        
        // Save changes
        await _context.SaveChangesAsync();

        // Return created order
        return _mapper.Map<OrderDto>(order);
    }
} 