using System.Net.Http.Json;
using BlazorApp2.DTOs;

namespace BlazorApp2.Services
{
    public class CartItemService 
    {
        private readonly HttpClient _httpClient;

        public CartItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CartItemDto>>("api/CartItem") 
                   ?? new List<CartItemDto>();
        }

        public async Task<CartItemDto?> GetCartItemByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CartItemDto>($"api/CartItem/{id}");
        }

        public async Task<CartItemDto?> AddToCartAsync(CreateCartItemDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/CartItem", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            return null;
        }

        public async Task<CartItemDto?> UpdateCartItemQuantityAsync(int id, UpdateCartItemDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/CartItem/{id}", dto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }
            return null;
        }

        public async Task<bool> RemoveFromCartAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/CartItem/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<OrderDto?> CheckoutAsync(CreateOrderDto orderDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Order/checkout", orderDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<OrderDto>();
            }
            return null;
        }
    }
}