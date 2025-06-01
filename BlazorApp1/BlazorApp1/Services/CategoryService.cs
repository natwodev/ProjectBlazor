using BlazorApp1.DTOs;

namespace BlazorApp1.Services;

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