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

    public async Task<CartItemDto> CreateAsync(CreateCartItemDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            throw new ArgumentException("Product not found");

        var cartItem = _mapper.Map<CartItem>(dto);
        cartItem.Price = product.Price;

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
} 