using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlazorApp2.Services;

public class CategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public async Task<JsonElement> GetAllCategoriesAsync()
    {
        var stream = await _http.GetStreamAsync("api/category");
        var doc = await JsonDocument.ParseAsync(stream);
        return doc.RootElement;
    }

    public async Task CreateCategoryAsync(CategoryDto category)
    {
        var response = await _http.PostAsJsonAsync("api/category", category);
        response.EnsureSuccessStatusCode();
    }

    public async Task PatchCategoryAsync(int id, Dictionary<string, object> patchData)
    {
        var json = JsonSerializer.Serialize(patchData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/category/{id}")
        {
            Content = content
        };

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteCategoryAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/category/{id}");
        response.EnsureSuccessStatusCode();
    }

}

public class CategoryDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
} 


