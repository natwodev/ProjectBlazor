using System.Net.Http.Json;
using System.Text.Json;
using BlazorApp2.DTOs;

namespace BlazorApp2.Services;

public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await _http.GetFromJsonAsync<List<ProductDto>>("api/product");
        return products ?? new List<ProductDto>();
    }
    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _http.GetFromJsonAsync<ProductDto>($"api/product/{id}");
        return product;
    }
    public async Task<HttpResponseMessage> CreateProductAsync(CreateProductDto newProduct)
    {
        return await _http.PostAsJsonAsync("api/product", newProduct);
    }
    
    public async Task<HttpResponseMessage> UpdateProductAsync(int id, UpdateProductDto updatedProduct)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, $"api/product/{id}")
        {
            Content = JsonContent.Create(updatedProduct)
        };

        return await _http.SendAsync(request);
    }
    
    public async Task<HttpResponseMessage> DeleteProductAsync(int id)
    {
        return await _http.DeleteAsync($"api/product/{id}");
    }
}