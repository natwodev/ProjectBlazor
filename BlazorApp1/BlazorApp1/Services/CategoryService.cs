using System.Net.Http.Json;
using BlazorApp1.DTOs;

public class CategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CategoryDto>> GetAllCategorysAsync()
    {
        var categorys = await _http.GetFromJsonAsync<List<CategoryDto>>("api/category");
        return categorys ?? new List<CategoryDto>();
    }
}

