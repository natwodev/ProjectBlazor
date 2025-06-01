using BlazorApp1.DTOs;

namespace BlazorApp1.Services;

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
}