using System.Text.Json;

namespace BlazorApp2.Services;

public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<JsonElement> GetAllProductsAsync()
    {
        var stream = await _http.GetStreamAsync("api/product");
        var json = await JsonDocument.ParseAsync(stream);
        return json.RootElement; // Trả về mảng JSON
    }
}